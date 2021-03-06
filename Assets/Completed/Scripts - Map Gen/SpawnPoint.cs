﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int SpawnID = 1;
    private bool message_sent = false;

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !message_sent)
        {
            SendMessageUpwards("SpawnEntered", SpawnID); //CALLING A LOT MORE THAN ONCE
            message_sent = true;
        }
    }

    public void SetID(int ID)
    {
        SpawnID = ID;
    }
}