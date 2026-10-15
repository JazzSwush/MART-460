using UnityEngine;

public class FruitCollectable : MonoBehaviour
{
    // animator for the fruit
    [SerializeField] private Animator animator;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if the player touches the fruit
        if (collision.CompareTag("Player"))
        {
            CollectFruit();
        }
    }

    private void CollectFruit()
    {
        // stop if fruit was already collected
        if (isCollected)
        {
            return;
        }

        isCollected = true;

        // add 1 to the UI counter
        FruitCounter.Instance.AddFruit();

        // play collected animation
        animator.Play("Collected");

        // turn off the fruit collider
        GetComponent<Collider2D>().enabled = false;

        // destroy fruit after 1 second
        Destroy(gameObject, 1f);
    }
}