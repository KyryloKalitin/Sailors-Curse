using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LosingMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _losingReasonInfo;
    [SerializeField] private TextMeshProUGUI _currentScore;
    [SerializeField] private TextMeshProUGUI _bestScore;

    [SerializeField] private Button _mainMenuButton;

    private SceneLoadService _sceneLoader;
    private InputService _inputService;

    [Inject]
    public void Construct(SceneLoadService sceneLoadService)
    {
        _sceneLoader = sceneLoadService;
    }

    private void Awake()
    {
        _inputService = new();
        _inputService.SetCursorMode(CursorMode.Menu);

        _mainMenuButton.onClick.AddListener(_mainMenuButtonOnClick);

        SetLosingReason();
        SetScores();

        GameProgressDataIO.DeleteGameProgressData();
    }

    private void SetScores()
    {
        var data = GameProgressDataIO.LoadData();

        int currentScore = data.DaysAmount;
        _currentScore.text = "Score: " + currentScore.ToString();

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if(bestScore > currentScore)
            _bestScore.text = "Best: " + bestScore.ToString();
        else
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            PlayerPrefs.Save();

            _bestScore.text = "Best: " + currentScore.ToString();
        }
    }

    private void SetLosingReason()
    {
        var losingResonSOSet = SOSetLoader.LoadLosingReasonSOSet();
        _losingReasonInfo.text = losingResonSOSet.GetCurrentLosingReasonDescription();
    }

    private void _mainMenuButtonOnClick()
    {
        _sceneLoader.Load(SceneLoadService.Scene.Menu);
    }

    private void OnDestroy()
    {
        _mainMenuButton.onClick.RemoveListener(_mainMenuButtonOnClick);
    }
}
