using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GemType {None, Heart,Cube,Diamond,Sphere }
public class StackableGem : MonoBehaviour
{
    public int gemCount = 1;
    public GemType gemType = GemType.None;
    public Transform top;
    public Vector3 targetScale=Vector3.one;  

    internal void GemCreated()
    { 
        transform.DOScale(targetScale*2f, 0.3f).OnComplete(() => {
            transform.DOScale(targetScale, 0.3f);
        });
    }

    internal void BoingBoingEffect()
    {
        transform.DOScale(targetScale*1.2f, 0.2f).OnComplete(()=>{
            transform.DOScale(targetScale, 0.1f);
        });
    }

    internal void CloseSlowly(Transform target) {
        transform.DOScale(0, 0.2f);
        transform.DOLocalMove(target.localPosition, 0.2f);
    }

}
