using DG.Tweening;
using UnityEngine;

public class ShipZoneUI : MonoBehaviour
{
    private bool _isAnimating = false;

    private Vector3 _defaultScale;

    private void Start()
    {
        _defaultScale = transform.localScale;

        Show();
    }

    public void Hide()
    {
        if (_isAnimating)
            return;

        if (!gameObject.activeSelf)
            return;

        _isAnimating = true;
        
        transform.DOScale(0f, 0.2f).OnComplete(() =>
        {
            gameObject.SetActive(false);

            transform.localScale = _defaultScale;

            _isAnimating = false;
        });
    }

    public void Show()
    {        
        if (_isAnimating)
            return;

        if (gameObject.activeSelf)
            return;

        _isAnimating = true;

        transform.localScale = Vector3.zero;

        gameObject.SetActive(true);

        transform.DOScale(_defaultScale, 0.2f).OnComplete(() => _isAnimating = false);
    }
}
