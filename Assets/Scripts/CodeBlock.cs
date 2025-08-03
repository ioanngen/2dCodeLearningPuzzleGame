using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    [TextArea]
    public string codeLine;

    public CodeBlock expectedNextBlock;
    public CodeBlock nextBlock;
}
