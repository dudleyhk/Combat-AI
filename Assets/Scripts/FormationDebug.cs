using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class FormationDebug : MonoBehaviour
{
    public Color leaderColour;
    public Color rifleColour;
    public Color fovColour;

    [Space]

    public Transform leader;
    public Transform rifleLeft;
    public Transform rifleRight;

    [Space]

    public float leaderOffset;
    public float rifleOffsetY;
    // public float watchOffset;








#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Transform tr = transform;
        Vector3 centre = tr.position;

        Vector3 leaderTargetPosition = centre + tr.forward * leaderOffset;
        Vector3 rifleLeftTargetPosition = centre - (tr.right * rifleOffsetY);
        Vector3 rifleRightTargetPosition = centre + tr.right * rifleOffsetY;

        Gizmos.color = leaderColour;
        Gizmos.color = rifleColour;
        Handles.color = fovColour;


        Gizmos.DrawWireSphere(leaderTargetPosition, .5f);
        Handles.DrawSolidArc(leaderTargetPosition + Vector3.up * 1.5f, Vector3.up, -leader.right + leader.forward, 90, 10f);

        Gizmos.DrawWireSphere(rifleLeftTargetPosition, .25f);
        Handles.DrawSolidArc(rifleLeftTargetPosition + Vector3.up * 1.5f, Vector3.up, -rifleLeft.right + rifleLeft.forward, 90, 10f);

        Gizmos.DrawWireSphere(rifleRightTargetPosition, .25f);
        Handles.DrawSolidArc(rifleRightTargetPosition + Vector3.up * 1.5f, Vector3.up, -rifleRight.right + rifleRight.forward, 90, 10f);

        Debug.DrawRay(leaderTargetPosition, leader.forward, Color.red);
        Debug.DrawRay(rifleLeftTargetPosition, rifleLeft.forward, Color.red);
        Debug.DrawRay(rifleRightTargetPosition, rifleRight.forward, Color.red);

        Debug.DrawRay(rifleLeftTargetPosition, leader.forward, Color.green);
        Debug.DrawRay(rifleRightTargetPosition, leader.forward, Color.green);


    }
#endif
}
