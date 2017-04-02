using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

    public float jumpHeight = 2.5f;
    public float timeToJumpApex = 0.25f;

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
            if (transform.parent)
                transform.parent.gameObject.GetComponent<Lift>().ToggleMovement();
        }
    }

}
