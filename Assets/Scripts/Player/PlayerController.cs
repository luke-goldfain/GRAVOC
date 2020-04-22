using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("The speed at which the character moves.")]
    private float moveSpeed;

    [SerializeField, Tooltip("The force with which the character jumps.")]
    private float jumpForce;

    [SerializeField, Tooltip("The influence the player has over the direction they are jumping mid-jump.")]
    private float airInfluence;

    [SerializeField, Tooltip("The sensitivity with which the camera rotates. A good default is 3.")]
    private float lookSensitivity;

    public enum MovementState
    {
        onGround, jumping, wallRunning
    }

    private MovementState currentMovState;
    private MovementState prevMovState;

    private Rigidbody rb;

    private float vAxis, hAxis;

    [HideInInspector]
    public GameObject PlayerCamera;

    private float yaw = 0;
    private float pitch = 0;

    private Vector3 collisionNormal;

    private float currentJumpRefreshTime;
    private float jumpRefreshTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();

        PlayerCamera = this.GetComponentInChildren<Camera>().gameObject;

        currentMovState = MovementState.jumping;
        prevMovState = currentMovState;
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        UpdateLookRotate();

        UpdateStateBehavior();

        if (currentMovState != prevMovState)
        {
            UpdateExecuteStateChangeActions();
        }
    }

    private void UpdateLookRotate()
    {
        yaw += lookSensitivity * Input.GetAxis("Mouse X");
        pitch -= lookSensitivity * Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.eulerAngles = new Vector3(0, yaw, 0);

        PlayerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }

    private void UpdateStateBehavior()
    {
        switch (currentMovState)
        {
            case MovementState.onGround:
                UpdateMoveGround();
                break;
            case MovementState.jumping:
                UpdateMoveJumping();
                break;
            case MovementState.wallRunning:
                UpdateMoveWallRun();
                break;
        }
    }

    private void UpdateExecuteStateChangeActions()
    {
        switch (currentMovState)
        {
            case MovementState.onGround:
                break;
            case MovementState.jumping:
                currentJumpRefreshTime = 0f;
                break;
            case MovementState.wallRunning:
                break;
        }

        prevMovState = currentMovState;

    }

    private void UpdateMoveGround()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8))) // Normal running behavior
        {
            if (hAxis != 0 || vAxis != 0)
            {
                rb.velocity = Vector3.ProjectOnPlane((transform.forward * vAxis * moveSpeed) + (transform.right * hAxis * moveSpeed), hit.normal);
            }
            else
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.2f);
            }
        }
        else
        {
            ChangeMovementState(MovementState.jumping);
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f)); // Add jumping force

            ChangeMovementState(MovementState.jumping);
        }
    }

    private void UpdateMoveJumping()
    {
        currentJumpRefreshTime += Time.deltaTime;

        if (hAxis != 0 || vAxis != 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, (transform.forward * vAxis * airInfluence) + (transform.right * hAxis * airInfluence), 0.02f);
        }

        if (currentJumpRefreshTime >= jumpRefreshTimer)
        {
            if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)))
            {
                ChangeMovementState(MovementState.onGround);
            }
            else
            {
                currentJumpRefreshTime = 0f;
            }
        }
    }

    private void UpdateMoveWallRun()
    {
        if (vAxis > 0)
        {
            // We have to check the angle the player is facing relative to the wall to determine the direction they should run
            if (Vector3.Angle(this.transform.forward, Vector3.Cross(Vector3.up, collisionNormal)) < Vector3.Angle(-this.transform.forward, Vector3.Cross(Vector3.up, collisionNormal)))
            {
                rb.velocity = moveSpeed * Vector3.Cross(Vector3.up, collisionNormal);
            }
            else
            {
                rb.velocity = -(moveSpeed * Vector3.Cross(Vector3.up, collisionNormal));
            }
        }
        else
        {
            ChangeMovementState(MovementState.jumping);
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce((collisionNormal * jumpForce) + (Vector3.up * jumpForce));

            ChangeMovementState(MovementState.jumping);
        }
    }

    private void ChangeMovementState(MovementState newState)
    {
        if (newState != currentMovState)
        {
            currentMovState = newState;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionNormal = collision.GetContact(0).normal;

        if (Mathf.Abs(collisionNormal.x + collisionNormal.z) > Mathf.Abs(collisionNormal.y) && currentMovState == MovementState.jumping) // If the collision is on a mostly vertical surface (a wall)
        {
            ChangeMovementState(MovementState.wallRunning);
        }
        else if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)) && hit.collider == collision.collider) // In other words, if the collider in question is the ground
        {
            ChangeMovementState(MovementState.onGround);
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (currentMovState != MovementState.onGround && Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)) && hit.collider == collision.collider)
    //    {
    //        ChangeMovementState(MovementState.onGround);
    //    }
    //}

    private void OnCollisionExit(Collision collision)
    {
        if (currentMovState != MovementState.jumping && !Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)))
        {
            ChangeMovementState(MovementState.jumping);
        }
    }
}
