using Dreamteck.Splines;
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

    void LoadNewLevel(SplineComputer splineComputer);
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
            }
        }

    }

    private Player _player;
    public Player player
    {
        get => _player; set => _player = value;
    }

    private UIManager _uiManager;
    public UIManager uiManager
    {
        get => _uiManager; set => _uiManager = value;
    }

    private CameraManager _cameraManager;
    public CameraManager cameraManager
    {
        get => _cameraManager; set => _cameraManager = value;
    }

    private PoolManager _poolManager;
    public PoolManager poolManager
    {
        get => _poolManager; set => _poolManager = value;
    }

    private LevelManager _levelManager;
    public LevelManager levelManager { get => _levelManager; set => _levelManager = value; }

    public void StartEndingProcess()
    {
        State = GameState.Success;
        EventManager.Get<OnGameSuccessEvent>().Execute();
        Debug.LogError("Game Ended ");
    }

    public void LoadNewLevel(SplineComputer splineComputer)
    {
        State = GameState.Pause;
        player.handAnimator.SetBool("GameEnded", false);
        player.SetNewSpline(splineComputer);
        player.ringController.ClearAllRings();

    }
}
