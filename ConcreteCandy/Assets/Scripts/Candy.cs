using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public LayerMask collisionMask;

    const float skinWidth = .015f; //The length of rayCast
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    //public float maxClimbAngle;
    //public float maxDescendAngle;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public CollisionInfo collisions;

    BoxCollider2D collidar;
    RaycastOrigins raycastOrigins;

    //public bool isLegCollidingWall;
    //public bool isHeadCollidingWall;
    public Player player;
    public bool above, below, left, right;



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

    // Use this for initialization
    void Start()
    {
        collidar = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        player = FindObjectOfType<Player>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //isLegCollidingWall = collisions.isLegCollidingWall;
        //isHeadCollidingWall = collisions.isHeadCollidingWall;
        above = collisions.above;
        below = collisions.below;
        //left = collisions.left;
        //right = collisions.right;

        if (collisions.above || collisions.below)
        {
            velocity.y = 0;
        }
        
        velocity.y += gravity * Time.deltaTime;
        Move(velocity * Time.deltaTime);
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        //collisions.velocityOld = velocity;

        //HorizontalCollisionsBefore(ref velocity);
        

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        //HorizontalCollisionsAfter(ref velocity);

        transform.Translate(velocity);
    }

    /*
    void HorizontalCollisionsBefore(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (velocity.x == 0)
        {
            directionX = 0;
        }

        for (int i = horizontalRayCount - 1; i >= 0; i--)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {

                if (i == horizontalRayCount - 1)
                {
                    collisions.isHeadCollidingWall = true;
                }

                if (i == 0 && Mathf.Sign(Input.GetAxisRaw("Horizontal")) == directionX)
                {
                    collisions.isLegCollidingWall = true;

                    if (collisions.isLegCollidingWall)
                    {
                        if (!collisions.isHeadCollidingWall)
                        {
                            if (Input.GetAxisRaw("Horizontal") != 0)
                            {
                                if (collisions.below)
                                {
                                    player.velocity.y = player.jumpVelocity;
                                }
                            }
                        }
                    }
                }

                //if (!collisions.climbingSlope)

                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;

            }
        }
    }

    void HorizontalCollisionsAfter(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (velocity.x == 0)
        {
            directionX = 0;
        }

        if (collisions.below)
        {
            player.velocity.y = player.jumpVelocity;
        }
    }
    */

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                if (hit.transform.gameObject != gameObject)
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }

                    collisions.above = directionY == 1;
                    collisions.below = directionY == -1;
                }
            }
        }
        
    }
    
    void UpdateRaycastOrigins()
    {
        Bounds bounds = collidar.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collidar.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below, left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public bool isLegCollidingWall;
        public bool isHeadCollidingWall;

        public void Reset()
        {
            isLegCollidingWall = false;
            isHeadCollidingWall = false;
            above = below = left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
