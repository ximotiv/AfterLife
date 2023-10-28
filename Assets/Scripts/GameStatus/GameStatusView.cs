using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameStatusView : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private GameStatusController _gameStatus;

    private void OnEnable()
    {
        _gameStatus.OnGameStarted += HideView;
        _gameStatus.OnGameStopped += ShowView;
    }

    private void OnDisable()
    {
        _gameStatus.OnGameStarted -= HideView;
        _gameStatus.OnGameStopped -= ShowView;
    }

    private void ShowView()
    {
        _startButton.gameObject.SetActive(true);
    }

    private void HideView()
    {
        _startButton.gameObject.SetActive(false);
    }

    [Inject]
    public void Construct(GameStatusController gameStatus)
    {
        _gameStatus = gameStatus;
    }
}
