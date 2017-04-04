﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask;

    const float skinWidth = .015f; //The length of rayCast
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public float maxClimbAngle;
    public float maxDescendAngle;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public CollisionInfo collisions;

    BoxCollider2D collidar;
    RaycastOrigins raycastOrigins;

    public bool isLegCollidingWall;
    public bool isHeadCollidingWall;
    public Player player;
    public bool above, below, left, right;

    // Use this for initialization
    void Start ()
    {
        collidar = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        player = FindObjectOfType<Player>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        isLegCollidingWall = collisions.isLegCollidingWall;
        isHeadCollidingWall = collisions.isHeadCollidingWall;
        above = collisions.above;
        below = collisions.below;
        left = collisions.left;
        right = collisions.right;
        //player.velocity.y += player.gravity * Time.deltaTime;
        //Move(player.velocity * Time.deltaTime);
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        //if (velocity.y < 0)
        //{
        //    DescendSlope(ref velocity);
        //}
        
        //if (velocity.x != 0)
        {
            HorizontalCollisionsBefore(ref velocity);
        }
        
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        HorizontalCollisionsAfter(ref velocity);
        //VerticalCollisions(ref velocity);

        if(player.GetComponent<LadderClimber>().IsClimbing)
        {
            velocity.y = 0;
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisionsBefore(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if(velocity.x == 0)
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
                //float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                //print("hit, " + Time.time + ", " + player.velocity);

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
                                //print("BEFOREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + Time.time + ", " + collisions.below);
                                if (collisions.below)
                                {
                                    //player.velocity.y = player.jumpVelocity;
                                    //print("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSStair                 BBBBBBBBBBBBBBBBBBBBBBBB, " + Time.time);
                                }
                            }
                        }
                    }
                }

                //if (!collisions.climbingSlope)

                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                //if (collisions.climbingSlope)
                //{
                //    velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                //}

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;

            }

            /*
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if(collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }

                    float distanceToSlopeStart = 0;

                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if(collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
            */
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
        //
        //for (int i = horizontalRayCount - 1; i >= 0; i--)
        //{
        //    Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
        //    rayOrigin += Vector2.up * (horizontalRaySpacing * i);
        //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
        //
        //    Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
        //
        //    if (hit)
        //    {
        //        if (i == horizontalRayCount - 1)
        //        {
        //            collisions.isHeadCollidingWall = true;
        //        }
        //
        //        if (i == 0 && Mathf.Sign(Input.GetAxisRaw("Horizontal")) == directionX)
        //        {
        //            collisions.isLegCollidingWall = true;
        //
        //            if (collisions.isLegCollidingWall)
        //            {
        //                if (!collisions.isHeadCollidingWall)
        //                {
        //                    if (Input.GetAxisRaw("Horizontal") != 0)
        //                    {
        //                        print("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfter" + Time.time + ", " + collisions.below);
                                if (collisions.below)
                                {
                                    //player.velocity.y = player.jumpVelocity;
                                    //print("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSStair                 AAAAAAAAAAAAAAAAAAAAAAAAAAAA, " + Time.time);
                                }
        //                    }
        //                }
        //            }
        //        }
        //        
        //        velocity.x = (hit.distance - skinWidth) * directionX;
        //        collisions.left = directionX == -1;
        //        collisions.right = directionX == 1;
        //
        //    }
        //    
        //}
    }

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
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.above = directionY == 1;
                collisions.below = directionY == -1;
            }
        }

        //if(collisions.climbingSlope)
        //{
        //    float directionX = Mathf.Sign(velocity.x);
        //    rayLength = Mathf.Abs(velocity.x) + skinWidth;
        //    Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight)+Vector2.up*velocity.y;
        //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
        //
        //    if(hit)
        //    {
        //        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
        //
        //        if(slopeAngle!=collisions.slopeAngle)
        //        {
        //            velocity.x = (hit.distance - skinWidth) * directionX;
        //            collisions.slopeAngle = slopeAngle;
        //        }
        //    }
        //}
    }

    /*
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    */

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
