using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int localX;
    private int localY;

    private SpriteRenderer rend;

    //random tex to choose between
    public Sprite[] tileTex;

    private void Start()
    {
    }

    // Use this for initialization
    private void OnEnable()
    {
        //rend = GetComponent<SpriteRenderer>();
        //int i = Random.Range(0, tileTex.Length);
        //rend.sprite = tileTex[i];
        //switch to random of 4 ground textures
    }

    public void SetPosition(int x, int y)
    {
        localX = x;
        localY = y;
    }
}