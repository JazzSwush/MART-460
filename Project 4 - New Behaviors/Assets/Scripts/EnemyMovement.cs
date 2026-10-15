using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] float speed = 3f;
    [SerializeField] private int startDirection = 1;

    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        halfWidth = spriteRenderer.bounds.extents.x;
        halfHeight = spriteRenderer.bounds.extents.y;

        currentDirection = startDirection;

        // flip enemy if starting direction is left
        if (startDirection == -1)
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move enemy
        movement.x = speed * currentDirection;
        movement.y = rigidBody.linearVelocity.y;

        rigidBody.linearVelocity = movement;

        SetDirection();
    }

    void SetDirection()
    {
        // check if enemy is moving right
        if (currentDirection == 1)
        {
            // check for wall on the right
            bool wallAhead = Physics2D.Raycast(enemyCollider.bounds.center, Vector2.right, enemyCollider.bounds.extents.x + 0.05f, LayerMask.GetMask("Ground"));

            // check for ground underneath the front of the enemy
            Vector2 groundCheckPosition = new Vector2(enemyCollider.bounds.max.x - 0.1f, enemyCollider.bounds.min.y + 0.05f);

            bool groundAhead = Physics2D.Raycast(groundCheckPosition, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));

            // turn around if there is a wall or no ground
            if (wallAhead || !groundAhead)
            {
                TurnAround();
            }
        }

        // check if enemy is moving left
        else if (currentDirection == -1)
        {
            // check for wall on the left
            bool wallAhead = Physics2D.Raycast(enemyCollider.bounds.center, Vector2.left, enemyCollider.bounds.extents.x + 0.05f, LayerMask.GetMask("Ground"));

            // check for ground underneath the front of the enemy
            Vector2 groundCheckPosition = new Vector2(enemyCollider.bounds.min.x + 0.1f, enemyCollider.bounds.min.y + 0.05f);

            bool groundAhead = Physics2D.Raycast(groundCheckPosition, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));

            // turn around if there is a wall or no ground
            if (wallAhead || !groundAhead)
            {
                TurnAround();
            }
        }
    }

    void TurnAround()
    {
        // change direction
        currentDirection *= -1;

        // flip enemy
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}