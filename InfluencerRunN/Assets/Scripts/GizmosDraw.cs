using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosDraw : MonoBehaviour
{
    public Color color;
    public Vector3 size;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, size);
    }
}

