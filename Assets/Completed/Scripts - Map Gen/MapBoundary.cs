using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundary : MonoBehaviour
{
    private ObjectPool tilepool;

    // Use this for initialization
    private void Start()
    {
        //tilepool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //if (col.gameObject.CompareTag("Tile"))
        //{
        //    tilepool.DisableOnExit(col.gameObject);
        //}
        //else
        //{
        //    Destroy(col.gameObject);
        //}
    }

    //private void Check
    //Lay out entire tile map with transforms

    //when player ~ transform distance < screenwidth
    //set active!
    //Instead of OnTriggerEnter, needs to be a positioning thing
    private void OnTriggerEnter2D(Collider2D col)
    {
        //GameObject obj = ObjectPool.SharedInstance.GetPooledObject("Tile");
        //if (obj != null)
        //{
        //    obj.transform.position = transform.position;
        //    obj.SetActive(true);
        //}
        //tilepool.EnableOnEnter(col.gameObject);
    }
}