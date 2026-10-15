using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    //player rigidbody2d
    [SerializeField] private Rigidbody2D rigidBody;

    // how strong the player can jump
    [SerializeField] private float jumpForce = 5f;

    // Player speed when sliding against wall
    [SerializeField] private float wallSlideSpeed = 2f;

    // player collider  used for raycasts
    [SerializeField] private Collider2D playerCollider;

    [SerializeField] private Animator animator;

    void Update()
    {
        // checks if player presses "space" or "w"
        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (GetIsGrounded()) // only jump if player is on ground
            {
                Jump();
            }
        }

        if(GetIsGrounded() && rigidBody.linearVelocity.y <= 0)
        {
            animator.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        // checks is player is touching a wall while in air
        if (GetIsTouchingWall() && !GetIsGrounded())
        {
            WallSlide(); // slows the player while sliding down wall
        }
    }

    private bool GetIsGrounded()
    {
        // raycast downwards to check for the ground
        RaycastHit2D hit = Physics2D.Raycast(playerCollider.bounds.center,Vector2.down,playerCollider.bounds.extents.y + 0.1f,LayerMask.GetMask("Ground"));

        return hit.collider != null; // returns true if raycast hits ground
    }

    private bool GetIsTouchingWall()
    {
        // raycasting for left and right to check for a wall
        RaycastHit2D leftHit = Physics2D.Raycast(playerCollider.bounds.center,Vector2.left,playerCollider.bounds.extents.x + 0.1f,LayerMask.GetMask("Ground"));

        RaycastHit2D rightHit = Physics2D.Raycast(playerCollider.bounds.center,Vector2.right,playerCollider.bounds.extents.x + 0.1f,LayerMask.GetMask("Ground"));

        return leftHit.collider != null || rightHit.collider != null; // return true if either side touches the ground layer
    }

    private void WallSlide()
    {
        // limit speed on how fast the player slides down the wall
        if (rigidBody.linearVelocity.y < -wallSlideSpeed)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, -wallSlideSpeed);
        }
    }

    private void Jump()
    {
        // apply the jump force while keeping player horizontal
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);

        animator.SetBool("isJumping", true);
    }
}