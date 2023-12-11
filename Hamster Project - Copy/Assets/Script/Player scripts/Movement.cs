using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private MoveJoyStick LjoyStick;
    [SerializeField] private MoveJoyStick RjoyStick;
    [SerializeField] private MoveJoyStick JumpJoystick;
    [SerializeField] private float fallMultiplier = 20;
    private Rigidbody2D body;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    private float wallJumpCooldown;
    [SerializeField] private float horizontalInput;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        GetInput();
        RotateCharacter();

        if (JumpJoystick.isPress){
                Jump();
        }
    }

    private void RotateCharacter()
    {
        if (horizontalInput > 0.01f){
            transform.localScale = Vector3.one;
            RunningAnim();
        }
        else if (horizontalInput < -0.01f){
            transform.localScale = new Vector3(-1, 1, 1);
            RunningAnim();
        }
        else{
            anim.SetBool("isRunning", false);
        }
    }

    private void RunningAnim()
    {
        anim.SetBool("isRunning", true);
    }

    private void GetInput()
    {
        if(LjoyStick.isPress )
        {
            horizontalInput = -1;
        }
        else if(RjoyStick.isPress)
        {
            horizontalInput = 1;
        }
        else
        {
            horizontalInput = 0;
        }
    }

    private void FixedUpdate(){
        Move();
        GravityCheck();
    }

    private void GravityCheck()
    {
        if (!isGrounded())
        {
            Vector2 gravity = new Vector2(0, -Physics2D.gravity.y);
            body.velocity -= gravity * fallMultiplier * Time.deltaTime;
        }
    }

    private void Move()
    {
        if (horizontalInput != 0)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
        else
        {
            // If no input, gradually slow down the character to simulate friction
            body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0f, Time.fixedDeltaTime * 10f), body.velocity.y);
        }
    }


    private void Jump()
    {
        if (isGrounded())
        {
            Debug.Log("Jump!!!");
            body.velocity = new Vector2(body.velocity.x , jumpPower);
            anim.SetTrigger("jump");
        }
    }



    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded();
    }
}