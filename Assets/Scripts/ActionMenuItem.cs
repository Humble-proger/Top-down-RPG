using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenuItem : MonoBehaviour, IPointerEnterHandler
{
    [Header("Reference")]
    [SerializeField] private Button _itemButton;
    [SerializeField] private TextMeshProUGUI _itemText;
    [SerializeField] private Image _selectionHighlight;

    [Header("Setting")]
    [SerializeField] private Color _normalColor = Color.black;
    [SerializeField] private Color _hoverColor = Color.gray;
    [SerializeField] private Color _selectedColor = Color.blue;
    [SerializeField] private bool _useAnimation = true;
    [SerializeField] private AnimationCurve _animationHandler;
    [SerializeField] private float _animationDuration;

    private int _index;
    private Action<int> _onHover;
    private Action<int> _onSelected;

    private void OnEnable()
    {
        _itemButton.onClick.AddListener(OnButtonSelect);
    }
    private void OnDisable()
    {
        _itemButton.onClick.RemoveListener(OnButtonSelect);
    }
    private void OnButtonSelect()
    {
        _onSelected?.Invoke(_index);
    }
    public void Initialize(string text, int index, 
        Action<int> onSelected, Action<int> onHover)
    {
        _itemText.text = text;
        _index = index;
        _onSelected = onSelected;
        _onHover = onHover;
    }
    public float GetHeight() {
        return _selectionHighlight.rectTransform.rect.height;
    }
    public void SetHover(bool check, bool useAnimation = true) {
        if (_useAnimation && useAnimation)
            StartCoroutine(ChangeColorSmoothly(check ? _hoverColor : _normalColor));
        else
            _selectionHighlight.color = check ? _hoverColor : _normalColor;
    }
    public void Select()
    {
        if (_useAnimation)
            StartCoroutine(ChangeColorSmoothly(_selectedColor));
        else
            _selectionHighlight.color = _selectedColor;
    }
    private IEnumerator ChangeColorSmoothly(Color toColor) {
        float delay = 0f;
        Color prevColor = _selectionHighlight.color;
        while (delay <= _animationDuration) {
            float normalizedvalue = delay / _animationDuration;
            _selectionHighlight.color = Color.Lerp(prevColor, toColor, _animationHandler.Evaluate(normalizedvalue));
            delay += Time.deltaTime;
            yield return null;
        }
    }
    public void OnPointerEnter(PointerEventData _) => _onHover?.Invoke(_index);

    public void Reset()
    {
        _itemText.text = string.Empty;
        _index = 0;
        _onHover = null;
        _selectionHighlight = null;
    }
}
