using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sequence _currentSequence;

    private Button _button;
    private Image _image;
    private TextMeshProUGUI _buttonText;

    private readonly Color _normalColor = new Color(0.4f, 0.4f, 0.4f, 1f);       
    private readonly Color _hoverColor = new Color(0.6f, 0.6f, 0.6f, 1f);        
    private readonly Color _pressedColor = new Color(0.3f, 0.3f, 0.3f, 1f);   

    private readonly Color _normalTextColor = Color.white;                     
    private readonly Color _pressedTextColor = new Color(0.8f, 0.8f, 0.8f, 1f); 

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = _button.GetComponent<Image>();
        _buttonText = _button.GetComponentInChildren<TextMeshProUGUI>();

        _image.color = _normalColor;
        _buttonText.color = _normalTextColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        _currentSequence?.Kill(true);

        _currentSequence = DOTween.Sequence()
            .Append(_image.DOColor(_hoverColor, 0.2f))
            .Join(_buttonText.DOColor(_normalTextColor, 0.2f))
            .Join(_image.transform.DOScale(1.05f, 0.2f).SetEase(Ease.OutQuad));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        _currentSequence?.Kill(true);

        _currentSequence = DOTween.Sequence()
            .Append(_image.DOColor(_normalColor, 0.2f))
            .Join(_buttonText.DOColor(_normalTextColor, 0.2f))
            .Join(_image.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        _currentSequence?.Kill(true);

        _currentSequence = DOTween.Sequence()
            .Append(_image.DOColor(_pressedColor, 0.05f))
            .Join(_buttonText.DOColor(_pressedTextColor, 0.05f)) 
            .Join(_image.transform.DOScale(0.95f, 0.05f))
            .Append(_image.DOColor(_hoverColor, 0.1f))
            .Join(_buttonText.DOColor(_normalTextColor, 0.1f)) 
            .Join(_image.transform.DOScale(1.05f, 0.1f).SetEase(Ease.OutQuad));
    }
}
