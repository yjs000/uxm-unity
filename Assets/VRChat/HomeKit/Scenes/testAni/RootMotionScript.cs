using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            Vector3 newPosition = transform.position;
            newPosition.z -= animator.GetFloat("Walkspeed") * Time.deltaTime;
            newPosition.x += animator.GetFloat("LTurnspeed") * Time.deltaTime;
            newPosition.x -= animator.GetFloat("RTurnspeed") * Time.deltaTime;
            transform.position = newPosition;
        }
    }
}
