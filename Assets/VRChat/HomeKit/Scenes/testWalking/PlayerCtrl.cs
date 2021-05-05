using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCtrl : MonoBehaviourPun
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float directionDampTime = .25f;

    //private float speed = 0.0f;
    private float h = 0.0f;
    private float v = 0.0f;
    private bool isStanding;

    RaycastHit hit;
    private readonly float MaxDistance = 350f; //Ray의 거리(길이)

    [Header("IK")]
    [Tooltip("If true then this script will control IK configuration of the character.")]
    public bool isIKActive = false;

    [Header("Interaction Offsets")]
    [SerializeField, Tooltip("An offset applied to the position of the character when they sit.")]
    float sittingOffset = -80f;

    //public Transform leftFootPosition = default;
    //public Transform rightFootPosition = default;

    private GameObject target;


    private void Start()
    {
        animator = GetComponent<Animator>();
        isStanding = true;

        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.Rotate(new Vector3(0, -45, 0));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.Rotate(new Vector3(0, 45, 0));
            }


            //speed = new Vector2(h, v).sqrMagnitude;

            animator.SetFloat("Speed", v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);


            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isStanding)
                {
                    //Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.yellow, 300f);
                    if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance) && hit.collider.gameObject.tag == "chair")
                    {
                        target = hit.collider.gameObject;
                        Vector3 pos = target.transform.position;
                        pos.z -= sittingOffset;

                        transform.position = pos;
                        transform.rotation = hit.collider.gameObject.transform.rotation;
                        isIKActive = true;
                        animator.SetBool("isSitting", true);
                        Debug.Log(hit.collider.gameObject.name);
                        isStanding = false;
                    }

                    else
                    {
                        Debug.Log("There's no chair to sit around you!");
                        return;
                    }
                }

                else
                {
                    animator.SetBool("isSitting", false);
                    isStanding = true;
                }
            }

            //if (animator) photonView.RPC("FlipRPC", RpcTarget.AllBuffered);
        }
        //else
        //{
        //    Debug.Log("photonView Error");
        //    return;
        //}
    }


    //[PunRPC]
    //void FlipRPC()
    //{
       
    //}

    //void OnAnimatorIK()
    //{
    //    if (!isIKActive) return;

    //    Debug.Log("gooood IK");

    //    if (rightFootPosition != null)
    //    {
    //        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
    //        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
    //        animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPosition.position);
    //        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootPosition.rotation);
    //    }
    //    if (leftFootPosition != null)
    //    {
    //        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
    //        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
    //        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPosition.position);
    //        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootPosition.rotation);
    //    }
    //}

}
