using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class ContainerConfig
{
    public RectTransform container;
    public bool requireCorrectOrder;
    public List<int> correctOrder = new List<int>();
}

public class BlockSequenceManager : MonoBehaviour
{
    public List<ContainerConfig> containers = new List<ContainerConfig>();
    private readonly Dictionary<RectTransform, List<BlockDragHandler>> containerBlocks = new();

    private void Start()
    {
        foreach (var cfg in containers)
        {
            if (cfg.container != null && !containerBlocks.ContainsKey(cfg.container))
                containerBlocks[cfg.container] = new List<BlockDragHandler>();
        }
    }

    public void OnBlockDragStart(BlockDragHandler block)
    {
        OnBlockRemovedFromContainer(block);
    }

    public RectTransform GetContainerUnderPointer(PointerEventData eventData)
    {
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
        {
            foreach (var cfg in containers)
            {
                if (cfg.container == null) continue;
                if (r.gameObject.transform == cfg.container || r.gameObject.transform.IsChildOf(cfg.container))
                    return cfg.container;
            }
        }
        return null;
    }

    public bool TryDropBlock(BlockDragHandler block, RectTransform container)
    {
        if (block == null || container == null) return false;

        if (!containerBlocks.ContainsKey(container))
            containerBlocks[container] = new List<BlockDragHandler>();

        var cfg = containers.Find(c => c.container == container);
        var list = containerBlocks[container];

        if (cfg != null && cfg.requireCorrectOrder)
        {
            int nextIndex = list.Count;
            if (nextIndex >= cfg.correctOrder.Count) return false;
            if (block.blockID != cfg.correctOrder[nextIndex]) return false;
        }

        if (!list.Contains(block))
            list.Add(block);

        StartCoroutine(SmoothCollapse(container));
        return true;
    }

    public void OnBlockRemovedFromContainer(BlockDragHandler block)
    {
        foreach (var kv in containerBlocks)
        {
            if (kv.Value.Remove(block))
            {
                StartCoroutine(SmoothCollapse(kv.Key));
                return;
            }
        }
    }

    private IEnumerator SmoothCollapse(RectTransform container)
    {
        LayoutGroup layout = container.GetComponent<LayoutGroup>();
        if (layout == null) yield break;

        float elapsed = 0f;
        float duration = 0.25f;
        Vector2 start = container.sizeDelta;

        LayoutRebuilder.ForceRebuildLayoutImmediate(container);
        Vector2 target = container.sizeDelta;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            container.sizeDelta = Vector2.Lerp(start, target, t);
            yield return null;
        }

        container.sizeDelta = target;
    }

    public void UpdateBlockOrder(BlockDragHandler block, RectTransform container)
    {
        if (block == null || container == null) return;
        if (!containerBlocks.ContainsKey(container))
            containerBlocks[container] = new List<BlockDragHandler>();

        var list = containerBlocks[container];
        list.Clear();

        for (int i = 0; i < container.childCount; i++)
        {
            var handler = container.GetChild(i).GetComponent<BlockDragHandler>();
            if (handler != null)
                list.Add(handler);
        }

        StartCoroutine(SmoothCollapse(container));
    }

    public IEnumerator FlashRedFeedback(BlockDragHandler block)
    {
        Image img = block.GetComponent<Image>();
        if (img == null) yield break;

        Color original = img.color;
        Vector3 originalScale = block.transform.localScale;

        for (int i = 0; i < 2; i++)
        {
            img.color = Color.red;
            block.transform.localScale = originalScale * 1.15f;
            yield return new WaitForSeconds(0.1f);

            img.color = original;
            block.transform.localScale = originalScale;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool ValidateAll()
    {
        foreach (var cfg in containers)
        {
            if (cfg == null || cfg.container == null) continue;

            if (!containerBlocks.ContainsKey(cfg.container)) return false;
            var list = containerBlocks[cfg.container];
            if (list.Count != cfg.correctOrder.Count) return false;

            for (int i = 0; i < cfg.correctOrder.Count; i++)
            {
                if (list[i].blockID != cfg.correctOrder[i])
                    return false;
            }
        }
        return true;
    }

    public void RunSequence()
    {
        bool success = ValidateAll();
        if (GameManager.instance != null)
            GameManager.instance.EndLevel(success);
    }
}
