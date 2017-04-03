using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {

    public bool moveSideways;
    public bool isMoving;
    Vector2 dir;
    public float speed;

    // Use this for initialization
    void Start()
    {
        isMoving = false;

        if (!moveSideways)
            dir = Vector2.up;
        else
            dir = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

            transform.parent.transform.Translate(dir * speed);

            if(!moveSideways)
            {
                if (pos.y >= 0.9f && dir != Vector2.down)
                {
                    dir = Vector2.down;
                }

                if (pos.y <= 0.1f && dir != Vector2.up)
                {
                    dir = Vector2.up;
                }
            }
            else
            {
                if (pos.x >= 0.7f && dir != Vector2.left)
                {
                    dir = Vector2.left;
                }

                if (pos.x <= 0.3f && dir != Vector2.right)
                {
                    dir = Vector2.right;
                }
            }
        }
    }

    public void ToggleMovement()
    {
        isMoving = !isMoving;
    }

    public void Shrink()
    {
        if (transform.localScale.x != 1)
        {
            transform.localScale = new Vector3(transform.localScale.x - 1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.transform.parent = transform.parent;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        coll.gameObject.transform.parent = null;
    }
}
