using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl2 : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float directionDampTime = .25f;

    //private float speed = 0.0f;
    private float h = 0.0f;
    private float v = 0.0f;
    private bool isStanding;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isStanding = true;

        if(!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animator)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            //speed = new Vector2(h, v).sqrMagnitude;

            animator.SetFloat("Speed", h*h + v*v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -45, 0));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 45, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isStanding)
            {
                animator.SetBool("isSitting", true);
                isStanding = false;
            }
            else
            {
                animator.SetBool("isSitting", false);
                isStanding = true;
            }
        }
    }
}
