using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptainsLogSingleItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemAmount;
    [SerializeField] private Image _itemIcon;

    public void SetValues(int amount, Sprite sprite)
    {
        if (amount > 0)
            _itemAmount.text = "+" + amount.ToString();
        else
            _itemAmount.text = amount.ToString();
        
        _itemIcon.sprite = sprite;
    }
}
