using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class BlockDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("Unique integer ID for this block")]
    public int blockID;

    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector2 originalAnchoredPos;

    RectTransform rect;
    CanvasGroup cg;
    Canvas canvas;
    BlockSequenceManager sequenceManager;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        sequenceManager = Object.FindFirstObjectByType<BlockSequenceManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalAnchoredPos = rect.anchoredPosition;
        cg.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cg.blocksRaycasts = true;

        bool snapped = false;
        if (sequenceManager != null)
        {
            snapped = sequenceManager.TrySnap(this);
        }

        if (!snapped)
        {
            transform.SetParent(originalParent);
            rect.anchoredPosition = originalAnchoredPos;
        }
    }
}
