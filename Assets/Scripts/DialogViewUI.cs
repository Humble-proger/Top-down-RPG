using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogViewUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Reference")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _itemPrefab;

    [Header("Setting")]
    [SerializeField] private int _visibleItem = 5;
    [SerializeField] private float _scrollSpeed = 10f;
    [SerializeField] private float _itemSpacing = 10f;
    [SerializeField] private bool _enableMouseWheel = true;

    private List<DialogScrollItem> _items = new();
    private int _currentIndex = 0;
    private float _targetPosition;
    private bool _isMouseOver;

    private void Awake()
    {
        InitializeItems();
        UpdateNavigation();
    }

    private void InitializeItems()
    {
        // Создаем тестовые элементы
        for (int i = 0; i < 10; i++)
        {
            CreateItem($"Item {i + 1}");
        }

        _targetPosition = _content.anchoredPosition.y;
    }

    private void CreateItem(string text)
    {
        GameObject itemObj = Instantiate(_itemPrefab, _content);
        
        if (itemObj.TryGetComponent(out DialogScrollItem item))
        {
            item.Initialize(text, _items.Count, OnItemSelected);
            _items.Add(item);
        }
    }

    public void ScrollPrevious()
    {
        if (_items.Count == 0) return;

        _currentIndex = Mathf.Max(0, _currentIndex - 1);
        UpdateNavigation();
    }

    public void ScrollNext()
    {
        if (_items.Count == 0) return;

        _currentIndex = Mathf.Min(_items.Count - 1, _currentIndex + 1);
        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        // Обновляем скролл позицию
        UpdateScrollPosition();

        // Обновляем выделение элементов
        UpdateSelection();
    }

    private void UpdateScrollPosition()
    {
        if (_items.Count == 0) return;

        float itemHeight = GetItemHeight();
        float visibleHeight = _visibleItem * (itemHeight + _itemSpacing);

        // Вычисляем целевую позицию для скролла
        float targetY = _currentIndex * (itemHeight + _itemSpacing);
        targetY = Mathf.Clamp(targetY, 0, Mathf.Max(0, _content.rect.height - visibleHeight));

        _targetPosition = targetY;
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].SetSelected(i == _currentIndex);
        }
    }

    private void SelectCurrentItem()
    {
        if (_currentIndex >= 0 && _currentIndex < _items.Count)
        {
            _items[_currentIndex].Select();
        }
    }

    private void OnItemSelected(int index)
    {
        _currentIndex = index;
        UpdateNavigation();
    }

    private float GetItemHeight()
    {
        if (_items.Count > 0)
            return _items[0].GetHeight();
        return 0f;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData _)
    {
        _isMouseOver = false;
    }

    // Публичные методы для управления
    public void AddItem(string text)
    {
        CreateItem(text);
        UpdateNavigation();
    }

    public void ClearItems()
    {
        foreach (var item in _items)
        {
            Destroy(item.gameObject);
        }
        _items.Clear();
        _currentIndex = 0;
        UpdateNavigation();
    }
}