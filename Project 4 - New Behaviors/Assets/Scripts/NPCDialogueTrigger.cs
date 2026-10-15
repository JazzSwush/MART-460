using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private SimpleDialogue dialogue;

    // if unchecked, the dialogue only plays the first time the player walks up
    [SerializeField] private bool repeatOnReenter = true;

    private bool hasPlayed = false;

    private void Reset()
    {
        // auto-fill the slot when the script is first added
        dialogue = GetComponent<SimpleDialogue>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
{

    if (!collision.CompareTag("Player"))
    {
        return;
    }

    if (!repeatOnReenter && hasPlayed)
    {
        return;
    }

    hasPlayed = true;
    dialogue.StartDialogue();
}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        // close the bubble when the player walks away
        if (dialogue != null)
        {
            dialogue.Close();
        }
    }
}