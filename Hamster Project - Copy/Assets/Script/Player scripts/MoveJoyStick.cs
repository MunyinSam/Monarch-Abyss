using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MoveJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPress = false;
    public int direction;
    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
    }
}