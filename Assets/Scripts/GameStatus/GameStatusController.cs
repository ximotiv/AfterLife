using System;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusController : MonoBehaviour
{
    public event Action OnGameStarted;
    public event Action OnGameStopped;

    [SerializeField] private Button _startButton;

    private bool _isGameStarted;

    public bool IsGameStarted => _isGameStarted;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        _isGameStarted = true;
        OnGameStarted?.Invoke();
    }

    public void StopGame()
    {
        _isGameStarted = false;
        OnGameStopped?.Invoke();
    }
}
