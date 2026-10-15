using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // how fast the enemy must move to count as running
    [SerializeField] private float runSpeedThreshold = 0.05f;

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        // avoid dividing by zero if the game is paused
        if (Time.deltaTime <= 0f)
        {
            return;
        }

        // how far the enemy moved horizontally since last frame
        float deltaX = transform.position.x - lastPosition.x;
        float speed = Mathf.Abs(deltaX) / Time.deltaTime;

        // true when moving, false when standing still
        animator.SetBool("isRunning", speed > runSpeedThreshold);

        // flip the sprite to face the direction it's moving
        if (deltaX > 0.001f)
        {
            spriteRenderer.flipX = false; // moving right
        }
        else if (deltaX < -0.001f)
        {
            spriteRenderer.flipX = true; // moving left
        }

        lastPosition = transform.position;
    }
}