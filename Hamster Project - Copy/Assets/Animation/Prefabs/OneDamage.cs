using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDamage : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(1);
        }
    }
}
