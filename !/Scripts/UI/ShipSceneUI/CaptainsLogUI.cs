using UnityEngine;
using Zenject;

public class CaptainsLogUI : MonoBehaviour
{
    [SerializeField] private Transform _cellContainer;
    [SerializeField] private Transform _cellTemplate;

    private GameEventManager _gameEventManager;

    [Inject]
    public void Construct(GameEventManager gameEventManager)
    {
        _gameEventManager = gameEventManager;
    }

    private void Start()
    {    
        UpdateVisual();               
    }

    private void UpdateVisual()
    {
        foreach (Transform child in _cellContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var gameEvent in _gameEventManager.LastGameEventsList)
        {
            Transform eventVisual = Instantiate(_cellTemplate, _cellContainer);
            eventVisual.gameObject.SetActive(true);

            eventVisual.GetComponent<CaptainsLogSingleCellUI>().SetEventSO(gameEvent);
        }
    }
}
