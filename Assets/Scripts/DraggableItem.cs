using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Vector3 _startPosition;
    private Transform _originanParent;
    private IDragSource<ItemData> _source;
    private Canvas _parentCanvas;

    private void Awake()
    {
        _source = GetComponentInParent<IDragSource<ItemData>>();
        _parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
        _originanParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(_parentCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _startPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetParent(_originanParent, true);

        IDragDestination<ItemData> container;

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            container = _parentCanvas.GetComponent<IDragDestination<ItemData>>();
        }
        else {
            container = GetContainer(eventData);
        }

        if (container != null) {
            TransferItemSystem.Instance.TransferItem(_source, container);
        }
    }
    private IDragDestination<ItemData> GetContainer(PointerEventData eventData)
    {
        List<RaycastResult> result = new();
        EventSystem.current.RaycastAll(eventData, result);

        foreach (RaycastResult raycastResult in result)
        {
            if (raycastResult.gameObject.TryGetComponent<IDragDestination<ItemData>>(out var slot))
            {
                return slot;
            }
        }
        return null;
    }
}