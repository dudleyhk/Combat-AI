using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoSphere : MonoBehaviour
{
    public Color col;
    public float radius;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = col;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
