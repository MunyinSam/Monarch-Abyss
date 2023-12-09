using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMobileInput : MonoBehaviour, IAgentInput
{

    public float speed = 5f;
    private CharacterController characterController;
    private MobileJoyStick mobileJoyStick;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        mobileJoyStick = GetComponent<MobileJoyStick>();
    }

    private void Update()
    {
        // Get the offset from the MobileJoyStick
        Vector2 offset = mobileJoyStick.GetOffset();

        // Use the offset to move the character
        MoveCharacter(offset);
    }

    private void MoveCharacter(Vector2 offset)
    {
        // Calculate the movement direction based on the offset
        Vector3 movementDirection = new Vector3(offset.x, 0, offset.y);

        // Normalize the movement direction to avoid faster diagonal movement
        movementDirection.Normalize();

        // Apply movement to the character controller
        characterController.Move(movementDirection * speed * Time.deltaTime);
    }
}
