using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTile : MonoBehaviour
{
    public Sprite[] tileTex;

    // Use this for initialization
    private void Awake()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        int i = Random.Range(0, tileTex.Length);
        rend.sprite = tileTex[i];
    }
}