using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

    public int score = 0;
    public float jumpHeight = 2.5f;
    public float timeToJumpApex = 0.25f;
    public int deathScoreDeductionFactor = 1;

    public float accelerationTimeAirorne = 0f;
    public float accelerationTimeGrounded = 0f;
    public float moveSpeed;

    //float gravity = -80;
    //float jumpVelocity = 20;
    public float gravity;
    public float jumpVelocity; //Do not change this value in the inspector! This is public only because other component needs to access it.

    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    GameObject platform;

	// Use this for initialization
	void Start ()
    {
        controller = GetComponent<Controller2D>();
        
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
	}
	
	// Update is called once per frame
	void Update ()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (controller.collisions.above || controller.collisions.below)
        {
            //if (!controller.collisions.isLegCollidingWall || (controller.collisions.isLegCollidingWall && controller.collisions.isHeadCollidingWall))
            {
                velocity.y = 0;
            }
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirorne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //print(velocity);

        if (Input.GetButtonDown("Horizontal"))
        {
            //print("button down, " + Time.time + "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
        }

        //if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        //{
        //    velocity.y = jumpVelocity;
        //}

        if (Input.GetButtonDown("Action"))
        {
            if (platform)
                platform.GetComponent<Lift>().ToggleMovement();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Candy"))
        {
            AddScore(GameManager.Instance.platforms.Length);
        }

        if (coll.gameObject.CompareTag("Platform"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Platform"))
        {
            platform = coll.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Platform"))
        {
            platform = null;
        }
    }

    public void AddScore(int val)
    {
        score += val;

        if (score < 0)
        {
            score = 0;
        }
    }

    void OnBecameInvisible()
    {
        if (!Camera.main)
            return;
        try
        {
            AddScore(-GameManager.Instance.platforms.Length * deathScoreDeductionFactor);

            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos = new Vector3(pos.x, 1.2f, pos.z);

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            velocity = Vector3.zero;

            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }

}
