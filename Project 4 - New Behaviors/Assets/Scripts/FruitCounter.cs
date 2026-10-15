using UnityEngine;
using TMPro;

public class FruitCounter : MonoBehaviour
{
    public static FruitCounter Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI fruitText;

    // static = these survive scene changes
    private static int levelStartFruits = 0; // fruits the player had when this level began
    private int fruitCount = 0;              // fruits right now

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // start the level with the fruits carried over from before
        fruitCount = levelStartFruits;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddFruit(int amount = 1)
    {
        fruitCount += amount;
        UpdateUI();
    }

    // call this right before loading the next level
    public void SaveProgress()
    {
        levelStartFruits = fruitCount;
    }

    // call this if you ever want to start the whole game over from zero
    public static void ResetAll()
    {
        levelStartFruits = 0;
    }

    private void UpdateUI()
    {
        fruitText.text = "x" + fruitCount;
    }
}