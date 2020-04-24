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
        onGround, jumping, wallRunning, wallClimbing
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

    private float currentWallClimbTime;

    [SerializeField, Tooltip("The maximum time in seconds for which the player can climb a wall.")]
    private float wallClimbTimer = 1f;

    private float wallClimbTopForce = 200f; // The force exerted on the player when they reach the top of a wall by climbing it

    private bool hasWallClimbedThisJump;

    private Vector3 airVelocity;

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
            case MovementState.wallClimbing:
                UpdateMoveWallClimb();
                break;
        }
    }

    private void UpdateExecuteStateChangeActions()
    {
        switch (currentMovState)
        {
            case MovementState.onGround:
                hasWallClimbedThisJump = false;
                break;
            case MovementState.jumping:
                currentJumpRefreshTime = 0f;
                break;
            case MovementState.wallRunning:
                break;
            case MovementState.wallClimbing:
                currentWallClimbTime = 0f;
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

        airVelocity = rb.velocity; // This is used in place of velocity when doing certain things, due to velocity getting changed before comparisons can be made (collisions etc)
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

    private void UpdateMoveWallClimb()
    {
        currentWallClimbTime += Time.deltaTime;

        if (currentWallClimbTime < wallClimbTimer && !hasWallClimbedThisJump)
        {
            if (vAxis > 0)
            {
                // Move the player upwards along the wall. Since this will not always be straight upwards, we do some cross multiplication to decide the "up" direction.
                rb.velocity = moveSpeed * Vector3.Cross(collisionNormal, Vector3.Cross(Vector3.up, collisionNormal));
            }
            else
            {
                hasWallClimbedThisJump = true;

                ChangeMovementState(MovementState.jumping);
            }

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(collisionNormal * jumpForce);
                
                ChangeMovementState(MovementState.jumping);
            }
        }
        else if (!hasWallClimbedThisJump)
        {
            currentWallClimbTime = 0f;

            hasWallClimbedThisJump = true;

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
            // Discern whether to have the player start wall running or wall climbing
            if (Vector3.Angle(collisionNormal, airVelocity) < 140f)
            {
                ChangeMovementState(MovementState.wallRunning);
            }
            else //if (rb.velocity.y > 0f) 
            {
                ChangeMovementState(MovementState.wallClimbing);
            }
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
        // If we climbed up a wall, add a forward impulse to simulate getting "over" the wall
        if (currentMovState == MovementState.wallClimbing)
        {
            rb.AddForce(-collisionNormal * wallClimbTopForce);
        }

        if (currentMovState != MovementState.jumping && !Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)))
        {
            ChangeMovementState(MovementState.jumping);
        }
    }
}
