using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour
{
    public float walkSpeed = 10.0f;
    public float rotationSpeed = 5.0f;

    float moveHorizontal;
    float moveVertical;

    bool isWalking;
    bool isRotating;

    void Awake()
    {
        Animator animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveVertical = Input.GetAxis("Vertical");
        if (moveVertical != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        moveHorizontal = Input.GetAxis("Horizontal");
        if(moveHorizontal!=0)
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        Turn();
        Walk();
    }
    void Walk()
    {
        if (isWalking == false) return;

        Vector3 movement = moveVertical * Vector3.forward * Time.deltaTime * walkSpeed;
        transform.Translate(movement);
    }

    void Turn()
    {
        if (isRotating == false) return;

        Vector3 rotation = new Vector3(0f, moveHorizontal * Time.deltaTime * rotationSpeed, 0f);
        transform.Rotate(rotation);
    }
}
