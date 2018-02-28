﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public GameObject tileHoverIcon;
    private Camera cam;
    private Canvas canvas;
    private bool editMode = false;

    private ObjectPool pool;

    // Use this for initialization
    private void Start()
    {
        cam = GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
        tileHoverIcon.SetActive(true);

        //MAPFILL
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        tileHoverIcon.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Vector2 origin = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //Vector2 origin = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
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

    public void EditModeSwitch()
    {
        editMode = !editMode;
        if (editMode)
        {
            cam.transform.SetParent(null);
            tileHoverIcon.SetActive(true);
        }
    }
}