using System.Collections.Generic;
using UnityEngine;

public class ProgramChecker : MonoBehaviour
{
    public CodeBlock startBlock;
    public List<string> correctLines;

    public void RunProgram()
    {
        List<string> userLines = new List<string>();
        CodeBlock current = startBlock;

        while (current != null)
        {
            userLines.Add(current.codeLine.Trim());
            current = current.nextBlock;
        }

        bool success = IsCorrect(userLines);
        GameManager.instance.EndLevel(success);
    }

    private bool IsCorrect(List<string> lines)
    {
        if (lines.Count != correctLines.Count) return false;

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i] != correctLines[i].Trim()) return false;
        }

        return true;
    }
}
