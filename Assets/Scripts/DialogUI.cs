using System.Collections;
using TMPro;
using UnityEngine;

public class DialogUI : MonoBehaviour 
{
    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _message;

    [Header("Settings")]
    [SerializeField] private float _charSpeed;

    private string _textTitle;
    private string _textMessage;
    private Coroutine _animationCoroutine;

    public void Initialize(string title, string message)
    {
        _textTitle = title;
        _textMessage = message;
    }

    public void ShowTitle() {
        _title.text = _textTitle;
    }

    public void PlayAnimationText()
    {
        SkipAnimation();
        _animationCoroutine = StartCoroutine(ShowMessageSlowly());
    }

    public void SetMessage(string newText) {
        _message.text = newText;
    }

    public void SkipAnimation() {
        if (IsAnimating) {
            StopCoroutine(_animationCoroutine);
            _message.text = _textMessage;
        }
    }

    private IEnumerator ShowMessageSlowly() {
        float charDelay = 1 / _charSpeed;
        int visibleChar = 0;

        _message.text = "";

        while (visibleChar < _textMessage.Length) {
            visibleChar++;
            string visibleText = _textMessage[..visibleChar];
            SetMessage(visibleText);
            yield return new WaitForSeconds(charDelay);
        }
    }

    public bool IsAnimating => _animationCoroutine != null;
}