using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float horizontal;
    [SerializeField] private float speed = 5;
    private Rigidbody2D rb;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
       horizontal = Input.GetAxisRaw("Horizontal");
    }
}
