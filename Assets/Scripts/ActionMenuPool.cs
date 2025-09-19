using System.Collections.Generic;
using UnityEngine;

public class ActionMenuPool : MonoBehaviour {
    [SerializeField] private ActionMenuItem _itemPrefab;
    [SerializeField] private RectTransform _container;

    private readonly Queue<ActionMenuItem> _actionMenuPool = new();

    public ActionMenuItem GetItem()
    {
        ActionMenuItem menuItem;
        if (_actionMenuPool.Count == 0)
        {
            menuItem = Instantiate(_itemPrefab, _container);
        }
        else
        {
            menuItem = _actionMenuPool.Dequeue();
            menuItem.gameObject.SetActive(true);
        }
        return menuItem;
    }

    public void PutItem(ActionMenuItem obj)
    {
        _actionMenuPool.Enqueue(obj);
        obj.Reset();
        obj.gameObject.SetActive(false);
    }

    public void Reset()
    {
        _actionMenuPool.Clear();
        foreach (Transform child in _container)
        {
            if (child.TryGetComponent(out ActionMenuItem bullet))
            {
                PutItem(bullet);
            }
        }
    }
}