using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuelBarPos : MonoBehaviour
{
    [SerializeField] Transform playerPos;

    void FixedUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(playerPos.transform.position);
    }
}
