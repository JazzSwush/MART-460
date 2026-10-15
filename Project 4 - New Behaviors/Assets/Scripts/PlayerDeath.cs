using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // game over and win UI panels
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private bool isFinalLevel = false;

    // keeps track of player if died or won
    private bool isDead = false;
    private bool hasWon = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if player touches a spike
        if (collision.gameObject.CompareTag("Spike"))
        {
            Die(); // kill player
        }

        // check if player touches an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die(); // kill player
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if player reaches/touches the trophy
        if (collision.CompareTag("Trophy"))
        {
            Win(); // player wins
        }
    }

    private void Die()
    {
        // stop if player has died or won
        if (isDead || hasWon)
        {
            return;
        }

        isDead = true; // set player as dead

        gameOverPanel.SetActive(true); // show the game over UI

        // disable player movements
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerJump>().enabled = false;

        // stops the player from moving
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    private void Win()
    {

    if (isDead || hasWon)
    {
        return;
    }

    hasWon = true;

    if (!isFinalLevel)
    {
        FruitCounter.Instance.SaveProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        return;
    }

        winPanel.SetActive(true); // show win UI

        // disables player movement
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerJump>().enabled = false;

        // stops player from moving
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void Retry()
{
    // reset the saved fruit count back to 0
    FruitCounter.ResetAll();

    // reload the scene to restart/play again
    SceneManager.LoadScene(0);
}
}