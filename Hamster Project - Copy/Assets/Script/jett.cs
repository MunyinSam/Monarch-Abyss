using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    private CapsuleCollider2D coll;
    [SerializeField] private float jetpackForce = 4f;
    [SerializeField] private float fuel = 100f;
    [SerializeField] private float fuelBurnRate = 10f;
    [SerializeField] private float fuelRefillRate = 5f;
    [SerializeField] private Button jetpackButton;
    [SerializeField] private Slider fuelSlider; // ปรับเพิ่ม Slider
    private float currentFuel;
    private bool isFlying = false;

    void Start()
    {
        coll = GetComponent<CapsuleCollider2D>();
        currentFuel = fuel;

        if (jetpackButton != null)
        {
            jetpackButton.onClick.AddListener(ToggleFlying);
        }

        // อัพเดตค่าเริ่มต้นของ Slider
        UpdateFuelSlider();
    }

    void Update()
    {
        if (isFlying && currentFuel > 0)
        {
            // บินเมื่อกำลังบินและยังมีพลังงาน
            ApplyJetpackForce();
            ConsumeFuel();
        }
        else if (IsGrounded())
        {
            // เพิ่มพลังงานเมื่อยืนบนพื้น
            RefillFuel();
        }

        // ตรวจสอบการลดพลังงาน
        if (isFlying)
        {
            currentFuel -= fuelBurnRate * Time.deltaTime;
            UpdateFuelSlider(); // อัพเดต Slider
        }

        // ตรวจสอบพลังงานไม่เป็นลบ
        currentFuel = Mathf.Max(currentFuel, 0f);
    }

    void ToggleFlying()
    {
        isFlying = !isFlying;
    }

    void ApplyJetpackForce()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, jetpackForce);
    }

    void ConsumeFuel()
    {
        currentFuel -= fuelBurnRate * Time.deltaTime;
    }

    void RefillFuel()
    {
        currentFuel += fuelRefillRate * Time.deltaTime;
        UpdateFuelSlider(); // อัพเดต Slider
    }

    void UpdateFuelSlider()
    {
        // อัพเดตค่าของ Slider
        if (fuelSlider != null)
        {
            fuelSlider.value = currentFuel / fuel;
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + 0.1f, jumpableGround);
        return hit.collider != null;
    }
}
