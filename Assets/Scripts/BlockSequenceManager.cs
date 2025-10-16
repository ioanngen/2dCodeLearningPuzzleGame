using UnityEngine;
using System.Collections.Generic;

public class BlockSequenceManager : MonoBehaviour
{
    [Header("Settings")]
    public bool requireCorrectOrder = true;
    public Transform snapContainer;

    [Header("Correct Order")]
    public List<int> correctOrder;

    private List<BlockDragHandler> snappedBlocks = new List<BlockDragHandler>();

    public bool TrySnap(BlockDragHandler block)
    {
        if (snappedBlocks.Contains(block))
            return false;

        int nextIndex = snappedBlocks.Count;
        bool canSnap = true;

        if (requireCorrectOrder)
        {
            if (nextIndex >= correctOrder.Count || block.blockID != correctOrder[nextIndex])
                canSnap = false;
        }

        if (canSnap)
        {
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
