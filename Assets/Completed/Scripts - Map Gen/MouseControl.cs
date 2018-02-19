using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public GameObject tileHoverIcon;
    private Camera cam;
    private Canvas canvas;
    private bool editMode = false;
    private GameObject bullet;
    private GameObject gun;
    private ObjectPool pool;

    // Use this for initialization
    private void Start()
    {
        cam = GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
        gun = GameObject.Find("Gun");
        //MAPFILL
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (editMode)
        {
            tileHoverIcon.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Vector2 origin = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("Ground"));

            if (hit)
            {
                tileHoverIcon.transform.position = hit.collider.transform.position;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hit.transform.parent.GetComponent<MapFill>().UserMapEdit((int)hit.collider.transform.localPosition.x, (int)hit.collider.transform.localPosition.y);
                }
            }
            else
            {
                // tileHoverIcon.transform.position = new Vector3(Input.GetAxis("Mouse X") * mouseSpeed, Input.GetAxis("Mouse Y") * mouseSpeed, 0);
                RaycastHit2D blankHit = Physics2D.Raycast(origin, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("Blank"));
                if (blankHit)
                {
                    tileHoverIcon.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        blankHit.transform.GetComponent<MapFill>().UserMapEdit((int)tileHoverIcon.transform.position.x, (int)tileHoverIcon.transform.position.y);
                    }
                }
            }
        }
        else // IF NOT IN EDIT MODE
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                bullet = pool.GetPooledObject("Bullet");
                bullet.transform.position = gun.transform.position;
                bullet.SetActive(true);
            }
        }
    }

    public void EditModeSwitch()
    {
        editMode = !editMode;
        if (editMode)
        {
            tileHoverIcon.SetActive(true);
        }
        else
        {
            tileHoverIcon.SetActive(false);
        }
    }
}