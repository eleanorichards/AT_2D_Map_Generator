using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int gCost;
    public int hCost;

    public int localX;
    public int localY;

    // Use this for initialization
    private void Start()
    {
    }

    public int fCost()
    {
        return gCost + hCost;
    }

    public void SetPosition(int x, int y)
    {
        localX = x;
        localY = y;
    }

    public bool TileLocation(int x, int y)
    {
        if (localX == x && localY == y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}