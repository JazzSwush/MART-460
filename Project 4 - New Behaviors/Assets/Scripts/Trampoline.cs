using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float bounceForce = 12f; //how strong bounce is

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Player collision with trampoline
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                //upward bounce
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);

                animator.Play("Bounce", 0, 0f); //plays animation from beginning everytime (not loop)
            }
        }
    }
}