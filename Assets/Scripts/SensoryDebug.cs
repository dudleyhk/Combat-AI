using UnityEngine;
using UnityEditor;


public class SensoryDebug : MonoBehaviour
{
    public Color col;



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Transform tr = transform;
        UnitMove unitMove = GetComponent<UnitMove>();


        Handles.color = col;
        Handles.DrawSolidArc(tr.position + Vector3.up * 1.5f, Vector3.up, -transform.right + transform.forward, 90, unitMove.rayDistance);




    }

#endif
}
