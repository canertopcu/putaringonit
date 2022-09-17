using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

public enum PoolType
{
    None=0,
    CubeGem = 1,
    DiamondGem = 5,
    SphereGem = 10,
    HeartGem = 15,
}
public class PoolManager : MonoBehaviour
{
    public static PoolType PoolTypeSelector(GemType gemType)
    {
        PoolType selectedPoolType = PoolType.None;
        switch (gemType)
        {
            case GemType.None:
                break;
            case GemType.Heart:
                selectedPoolType = PoolType.HeartGem;
                break;
            case GemType.Cube:
                selectedPoolType = PoolType.CubeGem;
                break;
            case GemType.Diamond:
                selectedPoolType = PoolType.DiamondGem;
                break;
            case GemType.Sphere:
                selectedPoolType = PoolType.SphereGem;
                break;
            default:
                break;
        }

        return selectedPoolType;
    }


    private IGameManager gameManager;
    [Inject]
    public void Setup(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    [System.Serializable]
    public class ObjectPool
    {
        public GameObject prefab;
        public int maximumInstances;
        public PoolType poolType;
        [HideInInspector]
        public Dictionary<int, GameObject> passiveObjectsDictionary;
        [HideInInspector]
        public GameObject pool;

        public void InitializePool()
        {
            passiveObjectsDictionary = new Dictionary<int, GameObject>();
            pool = new GameObject("[" + poolType + "]");
            DontDestroyOnLoad(pool);
            GameObject clone;

            for (int i = 0; i < maximumInstances; i++)
            {
                clone = Instantiate(prefab);
                clone.SetActive(false);
                clone.transform.SetParent(pool.transform);
                passiveObjectsDictionary.Add(clone.GetInstanceID(), clone);
            }
        }

        GameObject tempObject;
        public GameObject GetNextObject()
        {
            if (passiveObjectsDictionary.Count > 0)
            {
                tempObject = passiveObjectsDictionary.Values.ElementAt(0);
                passiveObjectsDictionary.Remove(passiveObjectsDictionary.Keys.ElementAt(0));
                return tempObject;
            }
            else
            {
                Debug.Log("PoolManager:"+ PoolType +"- passiveObjectsDictionary is empty. a new one is creating.");
                GameObject clone;
                clone = Instantiate(prefab);
                clone.SetActive(false);
                clone.transform.SetParent(pool.transform);
                passiveObjectsDictionary.Add(clone.GetInstanceID(), clone);

                tempObject = passiveObjectsDictionary.Values.ElementAt(0);
                passiveObjectsDictionary.Remove(passiveObjectsDictionary.Keys.ElementAt(0));
                return tempObject;
            }
        }

        public int MaximumInstances { get { return maximumInstances; } }
        public PoolType PoolType { get { return poolType; } set { poolType = value; } }
    }

    public List<ObjectPool> pools;

    private void Awake()
    {
        gameManager.poolManager = this;
    }
    private void Start()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].InitializePool();
        }
    }

    public GameObject Spawn(PoolType poolType, Transform parent = null)
    {
        return Spawn(poolType, null, null, parent);
    }

    public GameObject Spawn(PoolType poolType, Vector3? position, Quaternion? rotation, Transform parent = null)
    {
        ObjectPool pool = GetObjectPool(poolType);

        if (pool == null)
        {
            return null;
        }

        GameObject clone = pool.GetNextObject();

        if (clone == null)
        {
            return null;
        }

        if (parent != null)
        {
            clone.transform.SetParent(parent);
        }

        if (position != null)
        {
            clone.transform.position = (Vector3)position;
        }

        if (rotation != null)
        {
            clone.transform.rotation = (Quaternion)rotation;
        }

        clone.SetActive(true);
        return clone;
    }

    public GameObject Spawn(PoolType poolType, Vector3 minVector, Vector3 maxVector, Quaternion rotation)
    {
        // Determine the random position
        float x = Random.Range(minVector.x, maxVector.x);
        float y = Random.Range(minVector.y, maxVector.y);
        float z = Random.Range(minVector.z, maxVector.z);
        Vector3 newPosition = new Vector3(x, y, z);

        return Spawn(poolType, newPosition, rotation);
    }

    public void Despawn(PoolType poolType, GameObject obj)
    {
        ObjectPool poolObject = GetObjectPool(poolType);

        obj.transform.SetParent(poolObject.pool.transform);

        if (!poolObject.passiveObjectsDictionary.ContainsKey(obj.GetInstanceID()))
        {
            poolObject.passiveObjectsDictionary.Add(obj.GetInstanceID(), obj);
        }

        obj.SetActive(false);
    }

    public ObjectPool GetObjectPool(PoolType poolType)
    { 
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].PoolType == poolType)
            {
                return pools[i];
            }
        }

        return null;
    }

}
