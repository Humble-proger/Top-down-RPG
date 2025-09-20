using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

public class PlayerActionMenuView : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private ActionMenuPool _actionMenuPool;

    [Header("Setting")]
    [SerializeField] private float _itemSpacing = 10f;
    [SerializeField] private float _scrollSpeed = 100f;

    private readonly List<ActionMenuItem> _items = new();
    private int _currentIndex = 0;
    private int _currentIndexNode = 0;
    private Vector2 _targetPosition;
    private bool _coroutineWorked = false;
    private float _heightVisibleBlock = 0f;

    public event Action<int> Select;
    public event Action CancelDialog;

    private void OnEnable()
    {
        InputDealogState.DialogNext += ScrollDown;
        InputDealogState.DialogPrevious += ScrollUp;
        InputDealogState.DialogSelect += SelectItem;
        InputDealogState.DialogCancel += Cancel;
        InputDealogState.DialogScroll += MouseScroll;
    }

    private void OnDisable()
    {
        InputDealogState.DialogNext -= ScrollDown;
        InputDealogState.DialogPrevious -= ScrollUp;
        InputDealogState.DialogSelect -= SelectItem;
        InputDealogState.DialogCancel -= Cancel;
        InputDealogState.DialogScroll -= MouseScroll;
    }

    public bool NotInRange(int index) => index < 0 || index >= _items.Count;

    private void MouseScroll(float scroll)
    {
        float newTargetPositionY = Mathf.Clamp(_targetPosition.y - scroll * _scrollSpeed, 0f, _heightVisibleBlock - _content.rect.height);
        _targetPosition = new Vector2(_targetPosition.x, newTargetPositionY);
        if (!_coroutineWorked)
            StartCoroutine(ScrollContentSmooth());
    }

    public void InitializeItems(ref List<DialogueOption> options)
    {
        if (options.Count() == 0) {
            LoggerService.Error("(DialogViewUI) An attempt to create an empty dialog");
            return;
        }
        for (int optionIndex = 0; optionIndex < options.Count; optionIndex++) {
            DialogueOption option = options[optionIndex];
            if (!option.IsAvailable)
                continue;
            if (!string.IsNullOrEmpty(option.Text))
            {
                CreateItem(option.Text, optionIndex);
                LoggerService.Info("(DialogViewUI) One element has been added.");
            }
            else
            {
                LoggerService.Warning("(DialogViewUI) An attempt to add an empty message");
            }        
        }
        _heightVisibleBlock = (_items[0].GetHeight() + _itemSpacing) * _items.Count - _itemSpacing;
        _items[_currentIndex].SetHover(true, false);
    }

    private void CreateItem(string text, int index)
    {
        ActionMenuItem item = _actionMenuPool.GetItem();
        
        item.Initialize(text, index, _items.Count, OnSelect, OnHover);
        _items.Add(item);
    }

    private void OnHover(int indexNode, int indexItem)
    {
        if (NotInRange(indexItem) || indexItem == _currentIndex) return;
        _items[_currentIndex].SetHover(false);
        _currentIndex = indexItem;
        _currentIndexNode = indexNode;
        _items[indexItem].SetHover(true);
    }

    private void OnSelect(int indexNode, int indexItem)
    {
        if (NotInRange(indexItem))
        {
            LoggerService.Error("(DialogViewUI) index out of range!");
            return;
        }
        _items[indexItem].Select();
        Select?.Invoke(indexNode);
    }

    private void UpdateScrollPosition()
    {
        if (_items.Count == 0) return;
        
        float heightButton = _items[_currentIndex].GetHeight();
        float newPositionY = (heightButton + _itemSpacing) * _currentIndex;

        Vector2 currentPosition = _content.anchoredPosition;
        if (newPositionY < currentPosition.y)
        {
            _targetPosition = new Vector2(currentPosition.x, newPositionY);
            if (!_coroutineWorked)
                StartCoroutine(ScrollContentSmooth());
            return;
        }
        newPositionY += heightButton;
        if (newPositionY > currentPosition.y + _content.rect.height) {
            _targetPosition = new Vector2(currentPosition.x, Mathf.Clamp(newPositionY - _content.rect.height, 0, (heightButton + _itemSpacing) * _items.Count - _itemSpacing));
            if (!_coroutineWorked)
                StartCoroutine(ScrollContentSmooth());
        }
    }

    private IEnumerator ScrollContentSmooth() {
        _coroutineWorked = true;
        while (_content.anchoredPosition != _targetPosition) {
            _content.anchoredPosition = Vector2.Lerp(_content.anchoredPosition, _targetPosition, _scrollSpeed * Time.deltaTime);
            yield return null;
        }
        _coroutineWorked = false;
    }

    private void Cancel()
    {
        CancelDialog?.Invoke();
    }

    private void SelectItem()
    {
        OnSelect(_currentIndexNode, _currentIndex);
    }

    private void ScrollUp()
    {
        _items[_currentIndex].SetHover(false);
        _currentIndex--;
        if (_currentIndex < 0)
            _currentIndex = _items.Count - 1;
        _items[_currentIndex].SetHover(true);
        UpdateScrollPosition();
    }

    private void ScrollDown()
    {
        _items[_currentIndex].SetHover(false);
        _currentIndex++;
        if (_currentIndex > _items.Count - 1)
            _currentIndex = 0;
        _items[_currentIndex].SetHover(true);
        UpdateScrollPosition();
    }

    public void Reset()
    {
        foreach (var item in _items) {
            _actionMenuPool.PutItem(item);
        }
        _items.Clear();
        _currentIndex = 0;
        _currentIndexNode = 0;
    }
}