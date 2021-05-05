using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 movement;
    private Animator anim;
    private Rigidbody modelRigidbody;
    private void Start()
    {
        anim = GetComponent<Animator>();
        modelRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal") * -1.0f;
        float z = Input.GetAxis("Vertical") * -1.0f;
        movement = new Vector3(x, 0.0f, z).normalized * speed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (movement.magnitude < 0.01f)
        {
            anim.SetBool("isWalk", false);
            return;
        }
        modelRigidbody.velocity = movement;
        modelRigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
       
        anim.SetBool("isWalk", true);
    }
}
