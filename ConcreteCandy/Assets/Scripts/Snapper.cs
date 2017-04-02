using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Snap game object on grid

[ExecuteInEditMode]
public class Snapper : MonoBehaviour
{
#if UNITY_EDITOR

    void Update()
    {
        if (null == transform.parent)
        {
            var pos = transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            transform.position = pos;
        }
    }

#endif
}
