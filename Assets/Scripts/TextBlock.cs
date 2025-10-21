using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class TextBlock : MonoBehaviour
{
    public string[] validAnswers;

    private TMP_InputField inputField;
    private BlockDragHandler blockHandler;
    private int baseBlockID;
    private string lastText = "";

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        if (inputField == null)
            inputField = GetComponentInChildren<TMP_InputField>();

        blockHandler = GetComponent<BlockDragHandler>();
        if (blockHandler == null)
            blockHandler = GetComponentInParent<BlockDragHandler>();

        baseBlockID = blockHandler.blockID;

        lastText = inputField.text.Trim();
        ValidateText(lastText);
    }

    private void Update()
    {
        if (inputField == null || blockHandler == null)
            return;

        string current = inputField.text.Trim();

        if (current == lastText)
            return;

        lastText = current;
        ValidateText(current);
    }

    private void ValidateText(string input)
    {
        bool match = false;
        string normalized = input.ToLower();

        foreach (string valid in validAnswers)
        {
            if (!string.IsNullOrEmpty(valid) && normalized == valid.Trim().ToLower())
            {
                match = true;
                break;
            }
        }

        blockHandler.blockID = match ? baseBlockID : -1;

        if (inputField.image != null)
        {
            inputField.image.color = match
                ? new Color(0.8f, 1f, 0.8f)
                : new Color(1f, 0.8f, 0.8f);
        }
    }
}
