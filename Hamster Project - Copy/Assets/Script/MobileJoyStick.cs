using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    private RectTransform joystickTransform;

    [SerializeField]
    private float dragThreshold = 0.6f; // limit what to take in charMove
    [SerializeField]
    private int dragMovementDistance = 30;
    [SerializeField]
    private int dragOffsetDistance = 100;

    public event Action<Vector2> OnMove;


    private Vector2 currentOffset; // Store the current offset

    // Add a public method to get the offset value
    public Vector2 GetOffset()
    {
        return currentOffset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickTransform,
            eventData.position,
            null,
            out offset
        );

        // (-1) to 1
        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
        Debug.Log(offset);
        joystickTransform.anchoredPosition = offset * dragMovementDistance;

        currentOffset = offset; // Store the current offset

        Vector2 inputVector = CalculateMovementInput(offset);
        OnMove?.Invoke(inputVector);
        Debug.Log("OnMove event triggered with vector: " + inputVector);
    }

    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        float x = Mathf.Abs(offset.x) > dragThreshold ? offset.x : 0;
        float y = Mathf.Abs(offset.y) > dragThreshold ? offset.y : 0;
        return new Vector2(x, y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
     
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickTransform.anchoredPosition = Vector2.zero;
        OnMove?.Invoke(Vector2.zero);
    }

    private void Awake()
    {
        joystickTransform = (RectTransform)transform;
    }
}
