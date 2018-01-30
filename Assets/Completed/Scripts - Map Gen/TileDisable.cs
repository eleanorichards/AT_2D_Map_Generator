using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisable : MonoBehaviour
{
    // Use this for initialization
    private void OnSceneExit()
    {
        Destroy();
    }

    // Update is called once per frame
    private void Destroy()
    {
    }

    private void OnDisable()
    {
    }
}