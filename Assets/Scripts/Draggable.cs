using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = transform.position;
        canvas = GetComponentInParent<Canvas>();
    }
    /*
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); // Bring to front
    }*/

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CodeBlock myBlock = GetComponent<CodeBlock>();

        foreach (CodeBlock target in FindObjectsByType<CodeBlock>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (target == myBlock) continue;

            float snapDistance = 60f;
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < snapDistance)
            {
                if (target.expectedNextBlock != null && target.expectedNextBlock == myBlock)
                {
                    
                    Vector3 belowTarget = target.transform.position - new Vector3(0, target.GetComponent<RectTransform>().rect.height + 10f, 0);
                    transform.position = belowTarget;

                    target.nextBlock = myBlock;
                }
                else
                {
                    
                    transform.position = originalPosition;
                }

                return;
            }
        }
        transform.position = originalPosition;
    }
}
