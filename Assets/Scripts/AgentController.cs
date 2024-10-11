using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    //allagi 
    Animator animator;
    public float open = 100f;
    public float range = 10f;

    /*Rigidbody2D _rb;
    Vector2 _agentInput;*/
    public float _speed = 60f;
    void Start()
    {
       // _rb = GetComponent<Rigidbody2D>();   
    }
    //allagi
    private void Awake()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }


    private void Update()
    {
        // _agentInput = new Vector2(Input.GetAxisRaw("Horizontal") * _speed, Input.GetAxisRaw("Vertical") * _speed);
        bool isRunning = animator.GetBool("isRunning");
        bool forwardPressed = Input.GetKey("w");
        bool isTurningRight = animator.GetBool("isTurningRight");
        bool rightTurnPressed = Input.GetKey("d");
        bool isTurningLeft = animator.GetBool("isTurningLeft");
        bool leftTurnPressed = Input.GetKey("a");
        bool isJumping = animator.GetBool("isJumping");
        bool JumpPressed = Input.GetKey("space");
        //StartRunningForward
        if (!isRunning && forwardPressed)
        {
            animator.SetBool("isRunning", true);
        }
        //StopRunningForward
        if (isRunning && !forwardPressed)
        {
            animator.SetBool("isRunning", false);
        }

        //StartTurningRight
        if (rightTurnPressed && !isTurningRight)
        {
            animator.SetBool("isTurningRight", true);
        }
        //StopTurningRight
        if (!rightTurnPressed && isTurningRight)
        {
            animator.SetBool("isTurningRight", false);
        }
        //StopRunningForward & StartTurningRight
        if (rightTurnPressed && !isTurningRight && isRunning && !forwardPressed)
        {
            animator.SetBool("isTurningRight", true);
            animator.SetBool("isRunning", true);
        }
        //StartRunningForward & StopTurningRight
        if (!rightTurnPressed && isTurningRight && !isRunning && forwardPressed)
        {
            animator.SetBool("isTurningRight", false);
            animator.SetBool("isRunning", true);
        }

        //StartTurningLeft
        if (leftTurnPressed && !isTurningLeft)
        {
            animator.SetBool("isTurningLeft", true);
        }
        //StopTurningLeft
        if (!leftTurnPressed && isTurningLeft)
        {
            animator.SetBool("isTurningLeft", false);
        }
        //StopRunningForward & StartTurningLeft
        if (leftTurnPressed && !isTurningLeft && isRunning && !forwardPressed)
        {
            animator.SetBool("isTurningLeft", true);
            animator.SetBool("isRunning", true);
        }
        //StartRunningForward & StopTurningLeft
        if (!leftTurnPressed && isTurningLeft && !isRunning && forwardPressed)
        {
            animator.SetBool("isTurningLeft", false);
            animator.SetBool("isRunning", true);
        }
        //StartTurningLeft & StopTurningRight
        if (leftTurnPressed && !isTurningLeft && !rightTurnPressed && isTurningRight)
        {
            animator.SetBool("isTurningLeft", true);
            animator.SetBool("isTurningRight", false);
        }
        //StartTurningRight & StopTurningLeft
        if (!leftTurnPressed && isTurningLeft && rightTurnPressed && !isTurningRight)
        {
            animator.SetBool("isTurningLeft", false);
            animator.SetBool("isTurningRight", true);
        }
        //StartJumping
        if (!isJumping && JumpPressed)
        {
            animator.SetBool("isJumping", true);
        }
        //StopJumping
        if (isJumping && !JumpPressed)
        {
            animator.SetBool("isJumping", false);
        }
        //StartRunningForward & StartJumping
        if (!isJumping && JumpPressed && isRunning && !forwardPressed)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", true);
        }
        //StartTurningRight & StartJumping
        if (rightTurnPressed && !isTurningRight && !isJumping && JumpPressed)
        {
            animator.SetBool("isTurningRight", true);
            animator.SetBool("isJumping", true);
        }
        //StartTurningLeft & StartJumping
        if (leftTurnPressed && !isTurningLeft && !isJumping && JumpPressed)
        {
            animator.SetBool("isTurningLeft", true);
            animator.SetBool("isJumping", true);
        }
    }

    /*private void FixedUpdate()
    {
        _rb.AddForce(_agentInput);
    }*/

}
