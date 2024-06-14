using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartClick);
    }

    private void StartClick()
    {
        SceneManager.LoadScene("ShipScene");
    }
}
