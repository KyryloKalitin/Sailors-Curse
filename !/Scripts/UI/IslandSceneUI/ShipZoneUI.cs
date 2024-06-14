using UnityEngine;

public class ShipZoneUI : MonoBehaviour
{
    private void Start()
    {
        Show();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
