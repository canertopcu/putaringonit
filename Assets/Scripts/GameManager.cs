using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Pause, Play, Success, Failed }

public interface IGameManager
{
    GameState State { get; set; }
    void StartEndingProcess();

    Player player { get; set; }
    UIManager uiManager { get; set; }

    CameraManager cameraManager { get; set; }
    PoolManager poolManager { get; set; }
    LevelManager levelManager { get; set; }
}

public class GameManager : IGameManager
{
    public GameState _state = GameState.Pause;
    public GameState State
    {
        get => _state;

        set
        {
            if (value != _state)
            {
                _state = value;
                EventManager.Get<OnGameStateChanged>().Execute(value);
                Debug.LogError("Game State : " + value);
            }
        }

    }

    private Player _player;
    public Player player
    {
        get => _player; set => _player = value;
    }

    public UIManager _uiManager;
    public UIManager uiManager
    {
        get => _uiManager; set => _uiManager = value;
    }

    public CameraManager _cameraManager;
    public CameraManager cameraManager
    {
        get => _cameraManager; set => _cameraManager = value;
    }

    public PoolManager _poolManager;
    public PoolManager poolManager
    {
        get => _poolManager; set => _poolManager = value;
    }

    public LevelManager _levelManager;
    public LevelManager levelManager { get => _levelManager; set => _levelManager = value; }

    public void StartEndingProcess()
    {
        State = GameState.Success;
        Debug.LogError("Game Ended ");
    }
}
