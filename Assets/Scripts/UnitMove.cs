using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum Type
{
    None,
    Leader,
    Rifle,
    Watch
}





public class UnitMove : MonoBehaviour
{
    public Type type;
    public Transform goal;

    [Space]

    public float rayDistance;

    [Space]

    [SerializeField] private Vector3 smoothVelocity;
    [SerializeField] private float velocitySmoothing;



    private Transform tr;
    private Animator anim;
    private NavMeshAgent agent;

    private Vector3[] prevFootPos = new Vector3[2];
    private Transform[] footTransforms;




    private void Awake()
    {
        tr = transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();


        // Find the feet of the character
        footTransforms = new[]
        {
            anim.GetBoneTransform(HumanBodyBones.LeftFoot),
            anim.GetBoneTransform(HumanBodyBones.RightFoot)
        };
    }

    private void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }



    private void Update()
    {
        switch (type)
        {
            case Type.Leader: LeaderBehaviour(); break;
            case Type.Rifle: LeaderBehaviour(); break;
            case Type.Watch: LeaderBehaviour(); break;
            case Type.None:
            default:
                Debug.LogWarning("Agent type is None or invalid.");
                break;
        }
    }


    private void OnAnimatorMove()
    {
        Vector3 desiredVelocity = agent.desiredVelocity;
        desiredVelocity.y = 0f;

        Vector3 localDesiredVelocity = tr.InverseTransformDirection(desiredVelocity);
        smoothVelocity = Vector3.Lerp(smoothVelocity, localDesiredVelocity, velocitySmoothing > 0 ? Time.fixedDeltaTime / velocitySmoothing : 1f);

        //float speed = smoothVelocity.magnitude;
        //float angle = Vector3.SignedAngle(transform.forward, smoothVelocity, Vector3.up);
        //angle = ((angle - -180) / (180 - -180) * (1 - -1) + -1);

        anim.SetFloat("x", smoothVelocity.x);
        anim.SetFloat("z", smoothVelocity.z);

        Debug.DrawLine(tr.position, agent.nextPosition, Color.red);

        agent.nextPosition = tr.position;
        tr.position += anim.deltaPosition;
    }




    public void LeaderBehaviour()
    {
        if (RaycastForward(out RaycastHit info))
        {
            // analysis
        }


        agent.SetDestination(goal.position);


    }



    public bool RaycastForward(out RaycastHit info)
    {
        return Physics.Raycast(tr.position, tr.forward, out info, rayDistance);
    }



    private Vector3 RotatePointAround(Vector3 point, Vector3 around, Quaternion rotation)
    {
        return rotation * (point - around) + around;
    }

    private Quaternion RotateTowards(Vector3 dir, float maxDegrees)
    {
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            return Quaternion.RotateTowards(tr.rotation, targetRotation, maxDegrees);
        }
        else
        {
            return tr.rotation;
        }
    }


    private Vector3 CalculateBlendPoint()
    {
        // Fall back to rotating around the transform position if no feet could be found
        if (footTransforms[0] == null || footTransforms[1] == null) return tr.position;

        var leftFootPos = footTransforms[0].position;
        var rightFootPos = footTransforms[1].position;

        // This is the same calculation that Unity uses for
        // Animator.pivotWeight and Animator.pivotPosition
        // but those properties do not work for all animations apparently.
        var footVelocity1 = (leftFootPos - prevFootPos[0]) / Time.deltaTime;
        var footVelocity2 = (rightFootPos - prevFootPos[1]) / Time.deltaTime;
        float denominator = footVelocity1.magnitude + footVelocity2.magnitude;
        var pivotWeight = denominator > 0 ? footVelocity1.magnitude / denominator : 0.5f;
        prevFootPos[0] = leftFootPos;
        prevFootPos[1] = rightFootPos;
        var pivotPosition = Vector3.Lerp(leftFootPos, rightFootPos, pivotWeight);
        return pivotPosition;
    }
}
