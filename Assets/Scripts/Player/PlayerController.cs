using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("The player number for detecting input.")]
    public int PlayerNumber;

    [SerializeField, Tooltip("The speed at which the character moves.")]
    private float moveSpeed;

    [SerializeField, Tooltip("The force with which the character jumps.")]
    private float jumpForce;

    [SerializeField, Tooltip("The influence the player has over the direction they are jumping mid-jump.")]
    private float airInfluence;

    [SerializeField, Tooltip("The sensitivity with which the camera rotates. A good default is 3.")]
    private float lookSensitivity;

    public Animator anim;

    public enum MovementState
    {
        onGround, jumping, wallRunning, wallClimbing
    }

    private MovementState currentMovState;
    private MovementState prevMovState;

    private enum controlScheme
    {
        mkb, cont
    }

    private controlScheme currentControls;

    [HideInInspector]
    public Rigidbody rb { get; private set; }

    private float vAxis, hAxis;

    private Vector3 collisionNormal;

    private float currentJumpRefreshTime;
    private float jumpRefreshTimer = 0.5f;

    private float wallRunVerticalStart = 3f; // The starting value for the following variable
    private Vector3 wallRunVerticalModifier; // The vertical modifier for wall running, allowing for an arc-like wall-run

    private float currentWallClimbTime;

    [SerializeField, Tooltip("The maximum time in seconds for which the player can climb a wall.")]
    private float wallClimbTimer = 1f;

    private float wallClimbTopForce = 200f; // The force exerted on the player when they reach the top of a wall by climbing it

    private bool hasWallClimbedThisJump;

    private Vector3 airVelocity;

    [HideInInspector]
    public bool HitboxOnGround;

    [SerializeField, Tooltip("The camera attached to this player, to be assigned via prefab.")]
    public Camera PlayerCamera;

    private float yaw = 0;
    private float pitch = 0;

    [HideInInspector]
    public LocalPlayerSpawner PlayerSpawnerInstance;

    private ScoreManager scoreMgr;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();

        currentMovState = MovementState.jumping;
        prevMovState = currentMovState;

        currentControls = controlScheme.cont;

        scoreMgr = ScoreManager.Instance;

        HitboxOnGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("P" + PlayerNumber + "Horizontal");
        vAxis = Input.GetAxisRaw("P" + PlayerNumber + "Vertical");

        if (this.PlayerNumber == 1 && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
        {
            currentControls = controlScheme.mkb;
        }
        if (Input.GetAxis("P" + PlayerNumber + "AltHorizontal") != 0 || Input.GetAxis("P" + PlayerNumber + "AltVertical") != 0)
        {
            currentControls = controlScheme.cont;
        }

        UpdateLookRotate();

        UpdateStateBehavior();

        if (currentMovState != prevMovState)
        {
            UpdateExecuteStateChangeActions();
        }
    }

    private void UpdateLookRotate()
    {

        if (currentControls == controlScheme.mkb)
        {
            yaw += lookSensitivity * Input.GetAxis("Mouse X");
            pitch -= lookSensitivity * Input.GetAxis("Mouse Y");
        }
        else if (currentControls == controlScheme.cont)
        {
            yaw += lookSensitivity * Input.GetAxis("P" + PlayerNumber + "AltHorizontal");
            pitch += lookSensitivity * Input.GetAxis("P" + PlayerNumber + "AltVertical");
        }
         
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
                anim.SetBool("InAir", false);
                hasWallClimbedThisJump = false;
                break;
            case MovementState.jumping:
                anim.SetBool("InAir", true);
                currentJumpRefreshTime = 0f;
                break;
            case MovementState.wallRunning:
                wallRunVerticalModifier = wallRunVerticalStart * Vector3.Cross(collisionNormal, Vector3.Cross(Vector3.up, collisionNormal));
                break;
            case MovementState.wallClimbing:
                currentWallClimbTime = 0f;
                break;
        }

        prevMovState = currentMovState;

    }

    private void UpdateMoveGround()
    {
        if (HitboxOnGround && Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8))) // Normal running behavior
        {
            if (hAxis != 0 || vAxis != 0)
            {
                rb.velocity = Vector3.ProjectOnPlane((transform.forward * vAxis * moveSpeed) + (transform.right * hAxis * moveSpeed), hit.normal);
            }
            else
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.2f);
            }


            // Animation goodness
            float velX = transform.InverseTransformDirection(rb.velocity).x;
            float velZ = transform.InverseTransformDirection(rb.velocity).z;
            anim.SetFloat("VelX", velX / moveSpeed);
            anim.SetFloat("VelZ", -velZ / moveSpeed);
        }
        else if (HitboxOnGround)
        {
            if (hAxis != 0 || vAxis != 0)
            {
                rb.velocity = (transform.forward * vAxis * moveSpeed) + (transform.right * hAxis * moveSpeed);
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

        if (Input.GetButtonDown("P" + PlayerNumber + "Jump"))
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
            if (HitboxOnGround)//if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)))
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
                rb.velocity = moveSpeed * Vector3.Cross(Vector3.up, collisionNormal) + wallRunVerticalModifier;
            }
            else
            {
                rb.velocity = -(moveSpeed * Vector3.Cross(Vector3.up, collisionNormal)) + wallRunVerticalModifier;
            }

            wallRunVerticalModifier += Vector3.Cross(collisionNormal, Vector3.Cross(Vector3.down, collisionNormal)) * 0.05f;
        }
        else
        {
            ChangeMovementState(MovementState.jumping);
        }

        if (Input.GetButtonDown("P" + PlayerNumber + "Jump"))
        {
            rb.AddForce((collisionNormal * jumpForce) + (Vector3.up * jumpForce) + (Vector3.Normalize(rb.velocity) * jumpForce));

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

            if (Input.GetButtonDown("P" + PlayerNumber + "Jump"))
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

    public void ChangeMovementState(MovementState newState)
    {
        if (newState != currentMovState)
        {
            currentMovState = newState;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the collision is a projectile...
        if (collision.gameObject.tag == "Projectile")
        {
            Projectile projScript = collision.gameObject.GetComponent<Projectile>();

            // If the projectile was shot by a player other than this player...
            if (projScript.playerReference != null && projScript.playerReference != this)
            {
                // Hand this PlayerController over to the player spawner
                this.PlayerSpawnerInstance.SpawnPlayerAfterCooldown(this);

                // Tell the score manager to give a point to the player that shot the projectile
                scoreMgr.IncrementScore(projScript.playerReference.PlayerNumber);

                // Should expand on this once spawners are finished with object pooling and projectile respawning
                collision.gameObject.SetActive(false);

                // This one is fine, though
                this.gameObject.SetActive(false);
            }
        }
        else
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
            else if (HitboxOnGround)//if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.2f, ~(1 << 8)) && hit.collider == collision.collider) // In other words, if the collider in question is the ground
            {
                ChangeMovementState(MovementState.onGround);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Explosion")
        {
            this.rb.AddForce((Vector3.Normalize(this.transform.position - other.transform.position) + Vector3.up) * 400f);

            ChangeMovementState(MovementState.jumping);
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
    }

    public void AssignRotation(Vector3 euler)
    {
        pitch = euler.x;
        yaw = euler.y;
    }
}
