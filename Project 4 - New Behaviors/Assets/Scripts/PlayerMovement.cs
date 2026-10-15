using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    // player movement speed
    public float moveSpeed = 15f;

    // input action for player movement
    public string moveActionPath = "Player/Move";

    // animator for player animation
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private Collider2D playerCollider;

    // dust particle position
    [SerializeField] private Transform dustPoint;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private Vector2 moveInput;
    #endregion

    void Awake()
    {
        // gets the RigidBody2D component
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // prevents the player from rotating

        moveAction = InputSystem.actions.FindAction(moveActionPath); // finds the movement action from Input System
    }

    void Update()
    {
        // gets player's movement input
        moveInput = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;

        // Flip player
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // face right

            // move particles to back foot
            dustPoint.localPosition = new Vector3(-0.35f, -0.84f, 0);

            // make dust flow to the left
            dustParticles.transform.localRotation = Quaternion.Euler(0, 0, 0);
         }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // face left

    // move particles to back foot
    dustPoint.localPosition = new Vector3(0.35f, -0.84f, 0);

    // make dust flow to the right
    dustParticles.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

        // Animation
        if (moveInput.x != 0 && IsGrounded())
        {
            animator.SetBool("isRunning", true);

    // play dust particles when moving on the ground
        if (!dustParticles.isPlaying)
        {
            dustParticles.Play();
            }
        }
        else
        {
            animator.SetBool("isRunning", false);

        // stop creating new particles when not moving or in the air
        if (dustParticles.isPlaying)
        {
            dustParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    // check if the player is grounded
    // check if the player is grounded
    bool IsGrounded()
    {
        Vector2 rayStart = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, 0.1f, groundLayer);

        return hit.collider != null;
    }
}