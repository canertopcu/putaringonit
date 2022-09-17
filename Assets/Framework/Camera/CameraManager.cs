using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    private IGameManager gameManager;
    [Inject]
    public void Setup(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }


    public Animator cameraPassAnimator;

    public CinemachineVirtualCamera mainCamera;
      
    public float targetDamping = 0;


    private void Awake()
    {
        gameManager.cameraManager = this;
    }

    public void PassCamera()
    {
        cameraPassAnimator.SetTrigger("ChangeCamera");
    }

    public void ChangeZDaping(float zDamping, bool isImmediate = false)
    {
        if (isImmediate)
        {
            targetDamping = zDamping;
        }
        else
        {
            targetDamping = Mathf.Clamp(zDamping / 2f, 0f, 10f) + 3.33f;
        }
        mainCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = targetDamping ;
    }

    public float GetZDamping()
    {
        return targetDamping;
    }

    public void CameraShake(float intenstiy, float duration)
    {
        CinemachineBasicMultiChannelPerlin perlin = mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intenstiy;

        DOVirtual.DelayedCall(duration, () => perlin.m_AmplitudeGain = 0);


    }


}
