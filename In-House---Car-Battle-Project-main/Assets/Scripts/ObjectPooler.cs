using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tagName;
        public GameObject prefab;
        public int count;
    }

    public static ObjectPooler instance;
    [Header("Object Holder")]
    [SerializeField] Transform objectHolder;
    [Space]

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.count; i++  )
            {
                GameObject obj = Instantiate(pool.prefab, objectHolder);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tagName, objectPool);
        }
    }

    public void SpawnObject (string tagName, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = poolDictionary[tagName.ToString()].Dequeue();

        objectToSpawn.SetActive(false);
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tagName.ToString()].Enqueue(objectToSpawn);
    }
}
