using DG.Tweening;
using TMPro;
using UnityEngine;

public class Calendar : SelectableItem, IUntakeableItem
{
    [SerializeField] TextMeshProUGUI _daysText;

    private bool _isAnimating = false;

    protected override void Awake()
    {
        base.Awake();

        var data = GameProgressDataIO.LoadData();
        _daysText.text = "Day " + data.DaysAmount;

        _daysText.color = new Color(_daysText.color.r, _daysText.color.g, _daysText.color.b, 0f);
    }

    public void Interact()
    {
        if (!_isAnimating)
            Show();
    }

    private void Show()
    {
        _isAnimating = true;

        DOTween.Sequence()
            .Append(_daysText.DOFade(1f, 0.3f))
            .AppendInterval(1f)
            .Append(_daysText.DOFade(0f, 0.3f))
            .OnComplete(() => _isAnimating = false);
    }
}