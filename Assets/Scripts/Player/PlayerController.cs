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

    private GameObject playerCamera;

    private float yaw = 0;
    private float pitch = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();

        playerCamera = this.GetComponentInChildren<Camera>().gameObject;

        currentMovState = MovementState.jumping;
        prevMovState = currentMovState;
    }

    // Update is called once per frame
    void Update()
    {
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

        playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0);
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
                break;
            case MovementState.wallRunning:
                break;
        }

        prevMovState = currentMovState;

    }

    private void UpdateMoveGround()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

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
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        if (hAxis != 0 || vAxis != 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, (transform.forward * vAxis * airInfluence) + (transform.right * hAxis * airInfluence), 0.02f);
        }
    }

    private void UpdateMoveWallRun()
    {
        throw new NotImplementedException();
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
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)) && hit.collider == collision.collider) // In other words, if the collider in question is the ground
        {
            ChangeMovementState(MovementState.onGround);
        }

    }
}
