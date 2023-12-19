using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float fallMultiplier = 20;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private MoveJoyStick lJoystick;
    [SerializeField] private MoveJoyStick rJoystick;
    [SerializeField] private MoveJoyStick dashJoystick;
    [SerializeField] private MoveJoyStick jumpJoystick;

    [SerializeField] private float dashCooldown = 1f; // เวลาระหว่างการ dash
    [SerializeField] private float currentDashCooldown = 0;
    [SerializeField] private float dashDistance = 5f; // ระยะทางที่จะ Dash
    private Vector3 dashDirection;
    private Vector3 dashDestination;

    private Rigidbody2D body;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        currentDashCooldown += Time.deltaTime;

        GetInput();
        RotateCharacter();

        if (jumpJoystick.isPress){
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
        if(lJoystick.isPress )
        {
            horizontalInput = -1;
        }
        else if(rJoystick.isPress)
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
        Dash();
        GravityCheck();
    }

    private void Dash()
    {
        if ((currentDashCooldown >= dashCooldown && Input.GetKeyDown(KeyCode.LeftShift)) || (currentDashCooldown >= dashCooldown && dashJoystick.isPress))
        {
            // Set the dash direction based on the character's facing direction
            dashDirection = new Vector2(horizontalInput, 0);

            // Normalize the dash direction to ensure consistent speed
            dashDirection.Normalize();

            // Calculate the dash destination position
            dashDestination = transform.position + dashDirection * dashDistance;

            // Disable gravity during the dash
            body.gravityScale = 0;

            // Move the character to the dash destination
            StartCoroutine(PerformDash());

            // Reset the dash cooldown
            currentDashCooldown = 0;
        }
    }

    private IEnumerator PerformDash()
    {
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(transform.position, dashDestination);

        while (Vector3.Distance(transform.position, dashDestination) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * dashDistance;
            float fractionOfJourney = distCovered / journeyLength;

            // Move the character smoothly towards the dash destination
            transform.position = Vector3.Lerp(transform.position, dashDestination, fractionOfJourney);

            yield return null;
        }

        // Re-enable gravity after the dash is complete
        body.gravityScale = 1;
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