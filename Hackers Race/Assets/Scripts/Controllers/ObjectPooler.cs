using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }
    public bool hasGlobalSize;
    public float globalSize;
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;


    #region Singleton
    static private ObjectPooler instance;
    static public ObjectPooler Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    #endregion

    void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        #region Fill Dictionary
        foreach (Pool pool in Pools)
        {
            var poolContainer = transform.Find(pool.Tag);

            if (!poolContainer)
            {
                poolContainer = new GameObject(pool.Tag).transform;
                poolContainer.SetParent(transform, false);
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, poolContainer.transform);
                obj.SetActive(false);
                if (hasGlobalSize) obj.transform.localScale = Vector3.one * globalSize;
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.Tag, objectPool);
        }
        #endregion

    }

    /// <summary>
    /// Takes an element from the pool and spawns it in the scene
    /// </summary>
    /// <param name="position">Position where the element will be spawned</param>
    /// <param name="rotation">Rotation of the element that will be spawned</param>
    /// <param name="tag">Tag that indicates what kind of element will be searched in the dictionary</param>
    /// <returns></returns>
    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation, string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No existe pool con el tag: " + tag);
            return null;
        }
        if (PoolDictionary[tag].Count <= 0)
        {
            //Debug.LogWarning("No hay " + tag + " disponibles");
            foreach (Pool pool in Pools)
            {
                var poolContainer = transform.Find(pool.Tag);

                if (!poolContainer)
                {
                    poolContainer = new GameObject(pool.Tag).transform;
                    poolContainer.SetParent(transform, true);
                }
                if (pool.Tag == tag)
                {
                    GameObject toSpawn = Instantiate(pool.Prefab, poolContainer);
                    toSpawn.SetActive(false);
                    PoolDictionary[tag].Enqueue(toSpawn);
                    Debug.Log("se creo un nuevo prefab");
                    break;
                }
            }
        }
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();        
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);        
        return objectToSpawn;
    }

    /// <summary>
    /// Takes a random element from the pool and spawns it in the scene
    /// </summary>
    /// <param name="position">Position where the element will be spawned</param>
    /// <param name="rotation">Rotation of the element that will be spawned</param>
    /// <returns></returns>
    public GameObject SpawnRandomObjectFromPool(Vector3 position, Quaternion rotation)
    {
        string randomObject = Pools[UnityEngine.Random.Range(0, Pools.Count)].Tag;//Get random tag
        return SpawnFromPool(position, rotation, randomObject);
    }

    /// <summary>
    /// Deactivates all elements of a certain type
    /// </summary>
    /// <param name="tag">Tag that indicates the kind of element that will be deactivated</param>
    public void DeactivateAll(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No existe pool con el tag: " + tag);
            return;
        }
        else
        {
            var poolContainer = transform.Find(tag);
            foreach (Transform child in poolContainer)
            {
                child.gameObject.SetActive(false);
            }

        }
    }

    public void DeactivateAllPools()
    {
        foreach(string tag in PoolDictionary.Keys)
        {
            var poolContainer = transform.Find(tag);
            foreach (Transform child in poolContainer)
            {
                child.gameObject.SetActive(false);
            }
        }        
    }

    /// <summary>
    /// Adds an element to the pool's queue
    /// </summary>
    /// <param name="tag">tag that will be used to add to the dictionary</param>
    /// <param name="toEnqueue">Game object to enqueue</param>
    public void Enqueue(string tag, GameObject toEnqueue)
    {
        var poolContainer = transform.Find(tag);
        toEnqueue.transform.SetParent(poolContainer);
        PoolDictionary[tag].Enqueue(toEnqueue);
    }
}