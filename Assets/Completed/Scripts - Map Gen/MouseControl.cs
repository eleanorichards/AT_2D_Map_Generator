using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    // public GameObject tileHoverIcon;
    private Camera cam;

    private Canvas canvas;
    private bool editMode = false;

    private ObjectPool pool;
    private EditorMapFill map;

    // Use this for initialization
    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
        map = GameObject.Find("GameData").GetComponent<EditorMapFill>();
        //tileHoverIcon.SetActive(true);

        //MAPFILL
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
        //Vector3 origin = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, Camera.main.nearClipPlane);
        Vector3 origin = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));

        Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.farClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        Debug.DrawRay(origin, ray.direction, Color.red);
        // RaycastHit hit;

        if (hit)
        {
            if (transform.localPosition.x < map.width && transform.localPosition.y < map.height && transform.localPosition.x >= 0 && transform.localPosition.y >= 0)
            {
                transform.position = hit.collider.transform.position;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Debug.Log(hit.collider.transform.localPosition);
                    map.UserMapEdit((int)hit.collider.transform.localPosition.x, (int)hit.collider.transform.localPosition.y);
                }
            }
        }
    }

    public void EditModeSwitch()
    {
        editMode = !editMode;
        if (editMode)
        {
            cam.transform.SetParent(null);
            //tileHoverIcon.SetActive(true);
        }
    }
}