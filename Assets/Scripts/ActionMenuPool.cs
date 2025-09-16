using System.Collections.Generic;
using UnityEngine;

public class ActionMenuPool : MonoBehaviour {
    [SerializeField] private ActionMenuItem _itemPrefab;
    [SerializeField] private Transform _container;

    private Queue<ActionMenuItem> _actionMenuPool;

    public Bullet GetItem()
    {
        Bullet bullet;
        if (_bulletPool.Count == 0)
        {
            bullet = Instantiate(_bulletPrefab);
        }
        else
        {
            bullet = _bulletPool.Dequeue();
            bullet.gameObject.SetActive(true);
            bullet.Reset();
        }
        bullet.transform.parent = _container;
        return bullet;
    }

    public void PutItem(Bullet obj)
    {
        _bulletPool.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }

    public void Reset()
    {
        _bulletPool.Clear();
        foreach (Transform child in _container)
        {
            if (child.TryGetComponent(out Bullet bullet))
            {
                PutItem(bullet);
            }
        }
    }
}