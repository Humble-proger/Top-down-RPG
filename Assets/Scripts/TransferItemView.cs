using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransferItemView : MonoBehaviour {

    [SerializeField] private CanvasGroup[] _objectBlockRaycast;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _canceledButton;
    [SerializeField] private CanvasGroup _canvasGroupContainer;

    public event Action<int> Ñonfirm;
    public event Action Cancel;

    private bool[] _objectstate;

    private void Awake()
    {
        _slider.wholeNumbers = true;
        _slider.minValue = 1;
        _slider.maxValue = 2;
        int val = Mathf.RoundToInt(_slider.value);
        _slider.value = val;
        _valueText.text = val.ToString();
        _objectstate = new bool[_objectBlockRaycast.Length];
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        _canvasGroupContainer.blocksRaycasts = true;
        _slider.onValueChanged.AddListener(OnChangeSliderValue);
        _confirmButton.onClick.AddListener(OnConfitmButtonPressed);
        _canceledButton.onClick.AddListener(OnCancelButtonPressed);
        for (int i = 0; i < _objectBlockRaycast.Length; i++) {
            _objectstate[i] = _objectBlockRaycast[i].blocksRaycasts;
            _objectBlockRaycast[i].blocksRaycasts = false;
        }
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        _canvasGroupContainer.blocksRaycasts = false;
        _slider.onValueChanged.RemoveListener(OnChangeSliderValue);
        _confirmButton.onClick.RemoveListener(OnConfitmButtonPressed);
        _canceledButton.onClick.RemoveListener(OnCancelButtonPressed);
        for (int i = 0; i < _objectBlockRaycast.Length; i++)
            _objectBlockRaycast[i].blocksRaycasts = _objectstate[i];
        gameObject.SetActive(false);
    }

    private void OnCancelButtonPressed()
    {
        Cancel?.Invoke();
        Disable();
    }

    private void OnConfitmButtonPressed()
    {
        Ñonfirm?.Invoke((int) _slider.value);
        Disable();
    }

    private void OnChangeSliderValue(float arg0)
    {
        int intValue = Mathf.RoundToInt(arg0);
        _slider.value = intValue;
        UpdateTextSlider();
    }

    public void SetMaxSlider(int max) => _slider.maxValue = max;

    public void ResetSlider()
    {
        _slider.value = Mathf.RoundToInt(_slider.minValue);
        UpdateTextSlider();
    }
    private void UpdateTextSlider()
    {
        _valueText.text = ((int)_slider.value).ToString();
    }
}