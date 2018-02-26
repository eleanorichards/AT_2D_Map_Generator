using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    [Header("Pool Settings")]
    public GameObject objectToPool;

    public bool shouldExpand = true;
    public int poolSize = 100;
    public string itemName = "";
}

//acces grid index:
//totalWidth*height + curwidth

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pooledObject = new List<GameObject>();
    public List<ObjectPoolItem> itemsToPool;
    public static ObjectPool SharedInstance;

    private void Awake()
    {
        SharedInstance = this;
    }

    //Create tile array
    //set inactive
    //Add to tile list
    //Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
    private void Start()
    {
        //Crate all objects in pool
        pooledObject = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObject.Add(obj);
            }
        }
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
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObject.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    public void DeactivateObject(string tag)
    {
        for (int i = 0; i < pooledObject.Count; i++)
        {
            if (pooledObject[i].activeInHierarchy && pooledObject[i].CompareTag(tag))
            {
                pooledObject[i].SetActive(false);
            }
        }
    }

    public List<GameObject> ReturnActiveObjects(string tag)
    {
        List<GameObject> objList = new List<GameObject>();
        // int objNum = 0;
        for (int i = 0; i < pooledObject.Count; i++)
        {
            if (pooledObject[i].activeInHierarchy && pooledObject[i].CompareTag(tag))
            {
                objList.Add(pooledObject[i]);
            }
        }
        //objList.Sort();
        return objList;
    }
}

/* else //Level gen - needs work
                {
                    if (y > height - 5)
                    {
                        GameObject blank = GetPooledObject("Blank");
                        blank.transform.position = new Vector2(x, y);
                        blank.SetActive(true);
                    }
                    else
                    {
                        map[x, y] = rnd
                        .Next(0, 100);
                        if (map[x, y] < randomFillPercent)
                        {
                            GameObject tile = GetPooledObject("Tile");
                            tile.transform.position = new Vector2(x, y);
                            tile.SetActive(true);
                        }
                        else
                        {
                            GameObject blank = GetPooledObject("Blank");
                            blank.transform.position = new Vector2(x, y);
                            blank.SetActive(true);
                        }
                    }
                }*/