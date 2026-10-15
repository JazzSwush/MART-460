using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SimpleDialogue : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speaker;
        [TextArea(2, 5)] public string text;
        public Sprite portrait; // optional
    }

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Image portraitImage;

    [Header("Behavior")]
    [SerializeField] private float charsPerSecond = 45f;

    [Header("Input")]
    // Optional: drag in an action from your Input Actions asset (e.g. UI/Submit or Player/Interact).
    // If left empty, the fallback action below is used.
    [SerializeField] private InputActionReference advanceActionRef;

    // Fallback action with default bindings, editable in the Inspector.
    [SerializeField]
    private InputAction advanceAction = new InputAction(
        "Advance", InputActionType.Button);

    [Header("Content (author here for quick tests)")]
    [SerializeField] private DialogueLine[] lines;

    int index = -1;
    bool isTyping;
    string targetText;
    Coroutine typingCo;

    public bool IsOpen => panel != null && panel.activeSelf;

    InputAction Advance => advanceActionRef ? advanceActionRef.action : advanceAction;

    void Reset()
    {
        // Sensible defaults when the component is first added
        advanceAction = new InputAction("Advance", InputActionType.Button);
        advanceAction.AddBinding("<Keyboard>/space");
        advanceAction.AddBinding("<Keyboard>/enter");
        advanceAction.AddBinding("<Gamepad>/buttonSouth");
        advanceAction.AddBinding("<Mouse>/leftButton");
    }

    void Awake()
    {
        if (panel != null) panel.SetActive(false);

        // Make sure the fallback has at least one binding even if Reset() never ran
        if (advanceAction.bindings.Count == 0)
            advanceAction.AddBinding("<Keyboard>/space");
    }

    void OnEnable() => Advance?.Enable();
    void OnDisable() => Advance?.Disable();

    void Update()
    {
        if (!IsOpen) return;

        if (Advance != null && Advance.WasPressedThisFrame())
        {
            if (isTyping) SkipTyping();
            else Next();
        }
    }

    // Use this to populate dialogue at runtime (optional)
    public void Load(DialogueLine[] newLines)
    {
        lines = newLines;
    }

    // Call this to start (from trigger, button, etc.)
    public void StartDialogue()
    {

        if (panel == null || lines == null || lines.Length == 0) return;

        index = 0;
        panel.SetActive(true);
        ShowLine();
    }

    public void Close()
    {
        if (typingCo != null) StopCoroutine(typingCo);
        typingCo = null;
        isTyping = false;
        index = -1;

        // only hide the panel if it still exists
        if (panel != null) panel.SetActive(false);
    }

    void Next()
    {
        index++;
        if (index >= lines.Length) { Close(); return; }
        ShowLine();
    }

    void ShowLine()
    {
        var line = lines[index];

        if (nameText) nameText.text = line.speaker;
        if (portraitImage)
        {
            portraitImage.sprite = line.portrait;
            portraitImage.enabled = line.portrait != null;
        }

        StartTyping(line.text ?? "");
    }

    void StartTyping(string text)
    {
        targetText = text;
        if (typingCo != null) StopCoroutine(typingCo);
        typingCo = StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {
        isTyping = true;
        bodyText.text = "";
        float tPerChar = 1f / Mathf.Max(1f, charsPerSecond);
        for (int i = 0; i < targetText.Length; i++)
        {
            bodyText.text = targetText.Substring(0, i + 1);
            yield return new WaitForSeconds(tPerChar);
        }
        isTyping = false;
        typingCo = null;
    }

    void SkipTyping()
    {
        if (typingCo != null) StopCoroutine(typingCo);
        bodyText.text = targetText;
        isTyping = false;
        typingCo = null;
    }
}