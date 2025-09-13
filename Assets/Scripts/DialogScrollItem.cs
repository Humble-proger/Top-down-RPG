using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogScrollItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private Button _itemButton;
    [SerializeField] private Text _itemText;
    [SerializeField] private Image _selectionHighlight;
    [SerializeField] private Color _normalColor = Color.black;
    [SerializeField] private Color _hoverColor = Color.gray;
    [SerializeField] private Color _selectedColor = Color.blue;

    private int _index;
    private System.Action<int> _onSelected;

    public void Initialize(string text, int index, System.Action<int> onSelected)
    {
        _itemText.text = text;
        _index = index;
        _onSelected = onSelected;

        if (_itemButton != null)
        {
            _itemButton.onClick.AddListener(OnButtonClick);
        }

        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (_selectionHighlight != null)
        {
            _selectionHighlight.color = selected ? _selectedColor : _normalColor;
        }
    }

    public void Select()
    {
        Debug.Log($"Item selected: {_itemText.text}");
        // Здесь можно добавить логику при выборе элемента
    }

    private void OnButtonClick()
    {
        _onSelected?.Invoke(_index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onSelected?.Invoke(_index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_selectionHighlight != null)
        {
            _selectionHighlight.color = _hoverColor;
        }
    }

    public float GetHeight()
    {
        RectTransform rt = GetComponent<RectTransform>();
        return rt.rect.height;
    }

    private void OnDestroy()
    {
        if (_itemButton != null)
        {
            _itemButton.onClick.RemoveListener(OnButtonClick);
        }
    }
}