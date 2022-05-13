using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPlacement : MonoBehaviour
{
    //Script that handles IK implementation for the enemy animations from Mixamo.
    //Curve maps in the animations setup will handle the influence of the raycasts from the script to match the enemy feet against the uneven terrain.

    Animator anim;

    public LayerMask layermask;

    public float DistanceToGround = 0.05f;
    public float DistanceToGround2 = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));

            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layermask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    //Debug.Log("");
                    Vector3 footPosition = hit.point;
                    footPosition.y -= DistanceToGround2;
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                }
            }

            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layermask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    //Debug.Log("");
                    Vector3 footPosition = hit.point;
                    footPosition.y -= DistanceToGround2;
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                }
            }

        }
    }
}
