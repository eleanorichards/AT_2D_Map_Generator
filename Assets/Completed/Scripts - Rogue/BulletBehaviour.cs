using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float bulletSpeed = 5;
    private bool awake = false;

    public GameObject white_explosion;
    public GameObject black_explosion;

    // Use this for initialization
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (awake)
        {
            transform.Translate(Vector3.right * bulletSpeed);
        }
    }

    // Update is called once per frame
    private void OnEnable()
    {
        awake = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Tile"))
        {
            col.gameObject.SetActive(false);
            Invoke("StartWhiteExplosion", 0.1f);
            Invoke("StartBlackExplosion", 0.2f);
            //gameObject.SetActive(false);
        }
    }

    private void StartWhiteExplosion()
    {
        white_explosion.SetActive(true);
        DestroySmoke(1.0f);
    }

    private void StartBlackExplosion()
    {
        black_explosion.SetActive(true);
        DestroySmoke(1.5f);
    }

    private IEnumerator DestroySmoke(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}