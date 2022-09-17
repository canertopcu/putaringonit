using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollisionChecker : MonoBehaviour
{
    private IGameManager gameManager;
    [Inject]
    public void Setup(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Obstacle"))
        {
            gameManager.cameraManager.CameraShake(0.5f, 0.5f);
            gameManager.player.playerMovement.JumpBack();
        }

        if (other.CompareTag($"Merger"))
        {
            gameManager.cameraManager.CameraShake(0.5f, 0.5f);
            gameManager.player.ringController.MergeAllGems();
        }

        if (other.CompareTag($"Gem"))
        {
            gameManager.player.ringController.CreateNewGem(other.GetComponent<ICollectableGem>());
            Destroy(other.gameObject);
        }
    }
}
