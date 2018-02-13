using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public GameObject tileHoverIcon;
    private Camera cam;

    // Use this for initialization
    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        tileHoverIcon.SetActive(false);
        //MAPFILL
    }

    // Update is called once per frame
    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Vector2 origin = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("Ground"));

        if (hit)
        {
            tileHoverIcon.SetActive(true);

            tileHoverIcon.transform.position = hit.collider.transform.position;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log(hit.collider.transform.localPosition);
                hit.transform.parent.GetComponent<MapFill>().UserMapEdit((int)hit.collider.transform.localPosition.x, (int)hit.collider.transform.localPosition.y);
            }
        }
        else
        {
            tileHoverIcon.SetActive(false);
        }
    }
}