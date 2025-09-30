using UnityEngine;
using System.Collections.Generic;

public class BlockSequenceManager : MonoBehaviour
{
    [Header("Settings")]
    public bool requireCorrectOrder = true;  // toggle strict order
    public Transform snapContainer;          // where blocks snap (with LayoutGroup)

    [Header("Correct Order")]
    public List<int> correctOrder;           // list of expected blockIDs

    private List<BlockDragHandler> snappedBlocks = new List<BlockDragHandler>();

    public bool TrySnap(BlockDragHandler block)
    {
        // Prevent duplicate snap
        if (snappedBlocks.Contains(block))
            return false;

        int nextIndex = snappedBlocks.Count;
        bool canSnap = true;

        // If strict order enabled, check if block matches expected ID
        if (requireCorrectOrder)
        {
            if (nextIndex >= correctOrder.Count || block.blockID != correctOrder[nextIndex])
                canSnap = false;
        }

        if (canSnap)
        {
            // Parent under snapContainer and let VerticalLayoutGroup handle positioning
            block.transform.SetParent(snapContainer, false);
            snappedBlocks.Add(block);
            return true;
        }

        return false;
    }

    public bool ValidateSequence()
    {
        if (snappedBlocks.Count != correctOrder.Count)
            return false;

        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (snappedBlocks[i].blockID != correctOrder[i])
                return false;
        }
        return true;
    }

    public void RunSequence()
    {
        bool success = ValidateSequence();
        GameManager.instance.EndLevel(success);
    }

    // Optional: reset snapped blocks (e.g. on Try Again)
    public void ResetSequence()
    {
        foreach (var block in snappedBlocks)
        {
            if (block != null && block.originalParent != null)
            {
                block.transform.SetParent(block.originalParent);
                block.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
        snappedBlocks.Clear();
    }
}
