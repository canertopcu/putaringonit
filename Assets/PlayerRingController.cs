using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRingController : MonoBehaviour
{
    IGameManager gameManager;
    public List<StackableGem> gems;

    [SerializeField] private GameObject gemPlace;

    public void Setup(IGameManager gM)
    {
        gameManager = gM;
    }

    internal void CreateNewGem(ICollectableGem collectableGem)
    {
        StackableGem last = null;
        if (gems == null)
        {
            gems = new List<StackableGem>();
        }

        Transform gemParent = gemPlace.transform;
        if (gems.Count > 0)
        {
            gemParent = gems.Last().top;
            last = gems.Last();
        }

        StackableGem created = CreateStackableGem(collectableGem, gemParent);

        created.gemType = collectableGem.gemType;

        Vector3 localScale = Vector3.one;
        int gemCount = 1;

        if (last != null)
        {
            if (created.gemType == last.gemType)
            {
                localScale = last.transform.localScale;
                gemCount = last.gemCount;
            }
        }
        created.targetScale = localScale;
        created.gemCount = gemCount;
        gems.Add(created);
        StartCoroutine(WarnAllGems());
    }

    private StackableGem CreateStackableGem(ICollectableGem collectableGem, Transform gemParent)
    {
        PoolType selectedPoolType = PoolManager.PoolTypeSelector(collectableGem.gemType);
        StackableGem created = gameManager.poolManager.Spawn(selectedPoolType, gemParent.position, Quaternion.identity, gemPlace.transform).GetComponent<StackableGem>();
        created.collectable = collectableGem;
        return created;
    }

    IEnumerator WarnAllGems()
    {
        gems[gems.Count - 1].GemCreated();
        for (int i = 0; i < gems.Count; i++)
        {
            gems[gems.Count - i - 1].BoingBoingEffect();
            yield return new WaitForSeconds(0.1f);
        }
    }

    internal void ClearAllRings()
    {
        StopAllCoroutines(); 
        foreach (var item in gems)
        {
            Destroy(item.gameObject);
        }
        gems.Clear(); 
    }

    //public void MergeAllGems() {
    //    StartCoroutine(MergeAsync());
    //}
    public List<GemGroup> gemGroups;
    public void MergeAllGems()
    {
        gemGroups = new List<GemGroup>();
        GemType lastType = GemType.None;
        GemGroup gemGroup = new GemGroup();
        for (int i = 0; i < gems.Count; i++)
        {
            if (gems[i].gemType != lastType)
            {
                gemGroup = new GemGroup();
                lastType = gems[i].gemType;
                gemGroup.gemType = lastType;
                gemGroup.gems = new List<StackableGem>();
                gemGroup.gems.Add(gems[i]);
                gemGroups.Add(gemGroup);
            }
            else
            {
                if (gemGroup != null)
                {
                    gemGroup.gems.Add(gems[i]);
                }
            }
        }
        List<StackableGem> newGemList = new List<StackableGem>();
        foreach (var item in gemGroups)
        {
            newGemList.Add(item.gems[0]);

            for (int i = 1; i < item.gems.Count; i++)
            {
                item.gems[0].gemCount += item.gems[i].gemCount;
                var poolType = PoolManager.PoolTypeSelector(item.gems[i].gemType);
                gameManager.poolManager.Despawn(poolType, item.gems[i].gameObject);
                item.gems[0].targetScale += Vector3.one * 0.3f;
            }
            item.gems.Clear();

        }
        for (int i = 1; i < newGemList.Count; i++)
        {
            newGemList[i].transform.position = newGemList[i - 1].top.position;
        }

        gems.Clear();
        gems = newGemList;
        StartCoroutine(WarnAllGems());
    }

    public void Drop()
    {
        //for (int i = 0; i < (gems.Count >= 3 ? 3 : gems.Count); i++)
        //{
        //    StackableGem gem = gems.Last(); 
        //    gems.Remove(gem);
        //    var poolType = PoolManager.PoolTypeSelector(gem.gemType);
        //    gameManager.poolManager.Despawn(poolType, gem.gameObject);
            
        //}

    }
}
