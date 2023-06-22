using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject Prefab;
        public int Size;
    }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictinary;
    public static Pooler Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        poolDictinary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictinary.Add(pool.tag, objectPool);
        }
    }
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictinary.ContainsKey(tag))
            return null;

        GameObject objectToSpawn = poolDictinary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        poolDictinary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    //public void SetOff()
    //{
    //    foreach (var p in poolDictinary)
    //    {
    //        p.Value.
    //    }
    //}
}
