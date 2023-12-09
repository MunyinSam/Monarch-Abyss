using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jett : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;
    [SerializeField] private float jetpackForce = 4f;
    [SerializeField] private float fuel = 100f;
    [SerializeField] private float fuelBurnrate = 40f;
    [SerializeField] private float fuelRefillrate = 12f;
    private Animator anim;
    private float currentFuel;
    private bool haveFuel = true;
    private Rigidbody2D rb;
    public Slider fuelSlider; // Reference to the fuel slider

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentFuel = fuel;

        // Set the initial value of the fuel slider
        UpdateFuelSlider();
    }

    void Update()
    {
        // Use the jetpack when Space Bar is held down and there is enough fuel
        if (Input.GetButton("Jump") && haveFuel)
        {
            print("Flying");
            rb.velocity = new Vector2(rb.velocity.x, jetpackForce); // get the access
            currentFuel -= fuelBurnrate * Time.deltaTime;
            anim.SetBool("jett", true);
            UpdateFuelSlider(); // Update the fuel slider
        }
        else
        {
            anim.SetBool("jett", false);
        }

        // Check if fuel is critically low
        if (currentFuel <= 0.1f)
        {
            haveFuel = false;
        }
        else
        {
            haveFuel = true;
        }
    }


    private bool IsGrounded() // return true false for jumping
    {
        //print("IsGrounded = true");
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    void UpdateFuelSlider()
    {
        // Update the fuel slider value
        fuelSlider.value = currentFuel / fuel;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider has the "Fuel" tag
        if (other.CompareTag("Fuel"))
        {
            // Collect the fuel and increase the current fuel
            CollectFuel();

            // Destroy the collected fuel GameObject
            Destroy(other.gameObject);
        }
    }

    void CollectFuel()
    {
        // Increase the current fuel
        currentFuel = Mathf.Min(currentFuel + fuelRefillrate, fuel);

        // Update the fuel slider
        UpdateFuelSlider();

        // Optional: Add any other logic you want for collecting fuel
    }

}