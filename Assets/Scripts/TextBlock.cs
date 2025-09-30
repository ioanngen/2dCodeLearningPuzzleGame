using TMPro;
using UnityEngine;

[RequireComponent(typeof(BlockDragHandler))]
public class TextBlock : MonoBehaviour
{
    public TMP_InputField inputField;
    [TextArea] public string expectedText;

    public bool IsCorrect()
    {
        if (inputField == null) return false;
        return inputField.text.Trim() == expectedText.Trim();
    }
}
