using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool IsLadderTop = false;
    public bool IsLadderBottom = false;

    void OnDrawGizmos()
    {
        if (IsLadderTop || IsLadderBottom)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}
