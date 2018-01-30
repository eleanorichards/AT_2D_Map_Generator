using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public bool shouldExpand = true;
    public int poolSize = 100;
}

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pooledObject = new List<GameObject>();
    public List<ObjectPoolItem> itemsToPool;
    public static ObjectPool SharedInstance;

    private int width = 10;
    private int height = 10;

    private TileGenerator tileGen;

    private void Awake()
    {
        SharedInstance = this;
    }

    //Create tile array
    //set inactive
    //Add to tile list
    private void Start()
    {
        tileGen = GameObject.Find("TileGenerator").GetComponent<TileGenerator>();
        pooledObject = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject tile = (GameObject)Instantiate(item.objectToPool);
                tile.SetActive(false);
                pooledObject.Add(tile);
            }
        }
        tileGen.PlaceGrid();
    }

    //Return inactive tiles in the pool
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObject.Count; i++)
        {
            if (!pooledObject[i].activeInHierarchy && pooledObject[i].CompareTag(tag))
            {
                return pooledObject[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.CompareTag(tag))
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObject.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    //OnCollisionExit (attached to player) disable tile
    public void DisableOnExit(GameObject _tile)
    {
        if (_tile.tag == "Tile")
        {
            _tile.SetActive(false);
        }
        else
        {
            Destroy(_tile);
        }
    }

    public void EnableOnEnter(GameObject _tile)
    {
        if (_tile.tag == "Tile")
        {
            _tile.SetActive(true);
        }
    }
}

//public static TilePool instance;

///// <summary>
///// The object prefabs which the pool can handle.
///// </summary>
//public GameObject[] objectPrefabs;

///// <summary>
///// The pooled objects currently available.
///// </summary>
//public List<GameObject>[] pooledObjects;

///// <summary>
///// The amount of objects of each type to buffer.
///// </summary>
//public int[] amountToBuffer;

//public int defaultBufferAmount = 3;

///// <summary>
///// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
///// </summary>
//protected GameObject containerObject;

//private void Awake()
//{
//    instance = this;
//}

//// Use this for initialization
//private void Start()
//{
//    containerObject = new GameObject("ObjectPool");

//    //Loop through the object prefabs and make a new list for each one.
//    //We do this because the pool can only support prefabs set to it in the editor,
//    //so we can assume the lists of pooled objects are in the same order as object prefabs in the array
//    pooledObjects = new List<GameObject>[objectPrefabs.Length];

//    int i = 0;
//    foreach (GameObject objectPrefab in objectPrefabs)
//    {
//        pooledObjects[i] = new List<GameObject>();

//        int bufferAmount;

//        if (i < amountToBuffer.Length) bufferAmount = amountToBuffer[i];
//        else
//            bufferAmount = defaultBufferAmount;

//        for (int n = 0; n < bufferAmount; n++)
//        {
//            GameObject newObj = Instantiate(objectPrefab) as GameObject;
//            newObj.name = objectPrefab.name;
//            PoolObject(newObj);
//        }

//        i++;
//    }
//}

///// <summary>
///// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
///// then null will be returned.
///// </summary>
///// <returns>
///// The object for type.
///// </returns>
///// <param name='objectType'>
///// Object type.
///// </param>
///// <param name='onlyPooled'>
///// If true, it will only return an object if there is one currently pooled.
///// </param>
//public GameObject GetObjectForType(string objectType, bool onlyPooled)
//{
//    for (int i = 0; i < objectPrefabs.Length; i++)
//    {
//        GameObject prefab = objectPrefabs[i];
//        if (prefab.name == objectType)
//        {
//            if (pooledObjects[i].Count > 0)
//            {
//                GameObject pooledObject = pooledObjects[i][0];
//                pooledObjects[i].RemoveAt(0);
//                pooledObject.transform.parent = null;
//                pooledObject.SetActive(true);

//                return pooledObject;
//            }
//            else if (!onlyPooled)
//            {
//                return Instantiate(objectPrefabs[i]) as GameObject;
//            }

//            break;
//        }
//    }

//    //If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
//    return null;
//}

///// <summary>
///// Pools the object specified.  Will not be pooled if there is no prefab of that type.
///// </summary>
///// <param name='obj'>
///// Object to be pooled.
///// </param>
//public void PoolObject(GameObject obj)
//{
//    for (int i = 0; i < objectPrefabs.Length; i++)
//    {
//        if (objectPrefabs[i].name == obj.name)
//        {
//            obj.SetActiveRecursively(false);
//            obj.transform.parent = containerObject.transform;
//            pooledObjects[i].Add(obj);
//            return;
//        }
//    }
//}