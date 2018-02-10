using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int localX;
    public int localY;

    //random tex to choose between
    public Sprite[] tileTex;

    // Use this for initialization
    private void Awake()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        int i = Random.Range(0, 4);
        rend.sprite = tileTex[i];
        //switch to random of 4 ground textures
    }

    public void SetPosition(int x, int y)
    {
        localX = x;
        localY = y;
    }
}