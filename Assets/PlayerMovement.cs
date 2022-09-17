using DG.Tweening;
using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    IGameManager gameManager;
    [SerializeField] private GameObject childObject;
    private SplineFollower _splineController;

    private SplineFollower splineController
    {
        get
        {
            if (_splineController == null)
            {
                _splineController = GetComponentInChildren<SplineFollower>();
            }

            return _splineController;
        }
    }
    [SerializeField] private float horizontalMinLimit = -4f;
    [SerializeField] private float horizontalMaxLimit = 4f;
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float smtTime = 5;
    private float _firstHeightOfset = 0;

    public float editorSpeed = 10f;
    private float _speed = 10f;
    public float moveSpeed = 10f;

    public float Speed
    {
        set
        {
            if (Math.Abs(_speed - value) > 0)
            {
                _speed = value;
                splineController.followSpeed = value;
            }
        }
        get { return _speed; }
    }

    public event Action OnSpeedChanged;

    private float _horizontalValue = 0;

    private float jumpBackDistance = 10;

    public void Setup(IGameManager gM)
    {
        gameManager = gM;
    }

    private void OnValidate()
    {
        Speed = editorSpeed;
    }

    private void OnEnable()
    {
        InputManager.OnPointerMove += InputManager_OnPointerMove;
        EventManager.Get<OnGameStateChanged>().AddListener(Instance_OnGameStateChanged);
        splineController.onEndReached += SplineControllerOnEndReached;
    }

    private void OnDisable()
    {
        InputManager.OnPointerMove -= InputManager_OnPointerMove;
        EventManager.Get<OnGameStateChanged>().RemoveListener(Instance_OnGameStateChanged);
        splineController.onEndReached -= SplineControllerOnEndReached;

    }


    private void InputManager_OnPointerMove(Vector3 pointerPosition, Vector3 deltaPos)
    {
        if (gameManager.State != GameState.Play) return;
        _horizontalValue = Mathf.Clamp(_horizontalValue + deltaPos.x * Time.deltaTime * sensitivity,
            horizontalMinLimit, horizontalMaxLimit);

        childObject.transform.localPosition = Vector3.Lerp(childObject.transform.localPosition,
            new Vector3(_horizontalValue, 0, 0), Time.deltaTime * smtTime);
    }


    public void JumpBack()
    {
        float length = splineController.CalculateLength(0, 1);
        double jumpBack = (double)jumpBackDistance / (double)length;
        double targetPos = splineController.result.percent - jumpBack;

        DOTween.To(() => splineController.result.percent, (x) => splineController.result.percent = x, targetPos,
            0.5f).OnUpdate(() =>
            {
                childObject.transform.localPosition = Vector3.Lerp(childObject.transform.localPosition,
               new Vector3(_horizontalValue, 0, 0), Time.deltaTime * smtTime);
            });
    }

    private void SplineControllerOnEndReached(double obj)
    {
        Speed = 0;
        gameManager.StartEndingProcess();
        DOTween.To(() => _horizontalValue, (x) => _horizontalValue = x, 0, 0.5f).OnUpdate(() =>
        {
            childObject.transform.localPosition = new Vector3(_horizontalValue, 0, 0);
        }).OnComplete(() =>
        {
            gameManager.player.handAnimator.SetBool("GameEnded", true);
        });
    }

    void Start()
    {
        _firstHeightOfset = childObject.transform.position.y;
        Instance_OnGameStateChanged(gameManager.State);
    }


    internal void StartPlayer()
    {
        Speed = moveSpeed;
    }

    internal void StopPlayer()
    {
        Speed = 0;
    }


    private void Instance_OnGameStateChanged(GameState state)
    {
        if (state != GameState.Play)
        {
            StopPlayer();
        }
        else
        {
            StartPlayer();
        }
    }

    public void SetLimitation(float min, float max)
    {
        horizontalMinLimit = min;
        horizontalMaxLimit = max;
    }

    public void SetSpeed(float value, bool isMultiplier = false)
    {

        if (isMultiplier)
        {
            Speed *= value;
        }
        else
        {
            Speed = value;
        }
    }
}
