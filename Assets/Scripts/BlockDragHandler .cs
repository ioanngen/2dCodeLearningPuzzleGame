using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class BlockDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int blockID;

    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector2 originalAnchoredPos;

    private RectTransform rect;
    private Canvas canvas;
    private CanvasGroup cg;
    private BlockSequenceManager sequenceManager;

    private bool isSnapped;
    private bool hasMovedEnough;
    private Vector2 dragStartPos;
    private const float DRAG_THRESHOLD = 6f;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        sequenceManager = Object.FindFirstObjectByType<BlockSequenceManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
        hasMovedEnough = false;

        originalParent = transform.parent;
        originalAnchoredPos = rect.anchoredPosition;

        cg.blocksRaycasts = false;
        cg.alpha = 0.8f;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        sequenceManager?.OnBlockDragStart(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!hasMovedEnough)
        {
            if (Vector2.Distance(eventData.position, dragStartPos) > DRAG_THRESHOLD)
                hasMovedEnough = true;
            else
                return;
        }

        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cg.blocksRaycasts = true;
        cg.alpha = 1f;

        if (!hasMovedEnough)
        {
            ReturnToOriginal();
            return;
        }

        RectTransform targetContainer = sequenceManager?.GetContainerUnderPointer(eventData);

        if (targetContainer != null)
        {
            bool success = sequenceManager.TryDropBlock(this, targetContainer);
            if (success)
            {
                AudioManager.Instance.PlaySFX(SFXType.AttachBlock);
                transform.SetParent(targetContainer, false);
                sequenceManager.UpdateBlockOrder(this, targetContainer);
                isSnapped = true;
            }
            else
            {
                AudioManager.Instance.PlaySFX(SFXType.WrongBlock);
                StartCoroutine(sequenceManager.FlashRedFeedback(this));
                ReturnToOriginal();
            }
        }
        else
        {
            ReturnToOriginal();
        }

    }

    public void ReturnToOriginal()
    {
        transform.SetParent(originalParent, false);
        rect.anchoredPosition = originalAnchoredPos;
        isSnapped = false;
    }
}
