using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTile : MonoBehaviour
{
    private SpriteRenderer rend;

    //random tex to choose between
    public Sprite[] tileTex;

    private void Start()
    {
    }

    // Use this for initialization
    private void OnEnable()
    {
        rend = GetComponent<SpriteRenderer>();
        int i = Random.Range(0, tileTex.Length);
        rend.sprite = tileTex[i];
    }
}