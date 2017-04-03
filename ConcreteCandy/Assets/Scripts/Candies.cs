using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candies : MonoBehaviour
{

    // Use this for initialization
    public float fallSpeed = 8.0f;
    public float spinSpeed = 250.0f;
    public float aliveTime;
    float shrinkTimer;

    GameObject platform;
    
    void Update()
    {
        if(platform == null)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
        }
        else
        {
            if(Time.time >= shrinkTimer)
            {
                platform.GetComponent<Lift>().Shrink();
                Destroy(gameObject);
            }
        }
    }

    void OnBecameInvisible()
    {
        try
        {
            //Destroy(gameObject);
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos = new Vector3(pos.x, 1.2f, pos.z);

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (coll.gameObject.CompareTag("Platform"))
        {
            platform = coll.gameObject;
            shrinkTimer = Time.time + aliveTime;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Platform"))
        {
            platform = null;
        }
    }
}


