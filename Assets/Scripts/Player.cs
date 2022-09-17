using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using System.Linq;

public class Player : MonoBehaviour
{
    private IGameManager gameManager;
    [Inject]
    public void Setup(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public PlayerMovement playerMovement => GetComponent<PlayerMovement>();
    public PlayerRingController ringController => GetComponent<PlayerRingController>();

    public SplineFollower splineFollower;

    public Animator handAnimator;

    private void Awake()
    {
        gameManager.player = this;
        playerMovement.Setup(gameManager);
        ringController.Setup(gameManager);
    }

    // Start is called before the first frame update
    void Start()
    {
        handAnimator.SetBool("GameEnded", false);
    }

    public void SetNewSpline(SplineComputer splineComputer)
    {
        splineFollower.spline = splineComputer;
        splineFollower.Rebuild();
        splineFollower.SetDistance(0);
    }
}

[System.Serializable]
public class GemGroup
{
    public List<StackableGem> gems;
    public GemType gemType;
}