using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class LadderClimber : MonoBehaviour
{
    public float climbingSpeed = 1.0f;

    public bool CanClimbing { get { return laddersInUse.Count > 0; } }
    public bool IsClimbing { get { return climbing; } }

    private Player player;

    private float originalGravity = 0.0f;

    private HashSet<Ladder> laddersInUse = new HashSet<Ladder>();
    private HashSet<Ladder> ladderTopsInUse = new HashSet<Ladder>();
    private HashSet<Ladder> ladderBottomsInUse = new HashSet<Ladder>();
    private bool climbing = false;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (ladderBottomsInUse.Count > 0)
        {
            input.y = Mathf.Max(0.0f, input.y);
        }

        if (laddersInUse.Count > 0)
        {
            if (Mathf.Abs(input.y) > 0.001f)
            {
                if (!climbing)
                {
                    climbing = true;
                    originalGravity = player.gravity;
                    player.gravity = 0;
                    player.velocity.y = 0;
                    player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                }
                else
                {
                    transform.Translate(0.0f, input.y * climbingSpeed * Time.deltaTime, 0.0f);
                }
            }
        }
        else if (ladderTopsInUse.Count > 0)
        {
            if (input.y < 0.001f)
            {
                if (!climbing)
                {
                    climbing = true;
                    originalGravity = player.gravity;
                    player.gravity = 0;
                    player.velocity.y = 0;
                    player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                }
                else
                {
                    transform.Translate(0.0f, input.y * climbingSpeed * Time.deltaTime, 0.0f);
                }
            }
        }
        else if (climbing && 0 == ladderTopsInUse.Count)
        {
            climbing = false;
            player.gravity = originalGravity;
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        //print("climbing: " + climbing + "  ladders: " + laddersInUse.Count);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var ladder = other.GetComponent<Ladder>();
        if (null != ladder)
        {
            if (ladder.IsLadderTop)
                ladderTopsInUse.Add(ladder);
            else if (ladder.IsLadderBottom)
                ladderBottomsInUse.Add(ladder);
            else
                laddersInUse.Add(ladder);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var ladder = other.GetComponent<Ladder>();
        if (null != ladder)
        {
            if (ladder.IsLadderTop)
                ladderTopsInUse.Remove(ladder);
            else if (ladder.IsLadderBottom)
                ladderBottomsInUse.Remove(ladder);
            else
                laddersInUse.Remove(ladder);
        }
    }
}
