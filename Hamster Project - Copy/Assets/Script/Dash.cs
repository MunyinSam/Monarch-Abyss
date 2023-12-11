using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashCooldown = 1f; // เวลาระหว่างการ dash
    private float currentCooldown;
    [SerializeField] private Animator animator;
    [SerializeField] private Button dashButton; // ปุ่ม Dash
    [SerializeField] private float dashDistance = 5f; // ระยะทางที่จะ Dash
    private bool isDashing = false;

    private void Start()
    {
        currentCooldown = 0f;

        if (dashButton != null)
        {
            dashButton.onClick.AddListener(DashFromUI);
        }
    }

    private void Update()
    {
        if (CanDash() && Input.GetKeyDown(KeyCode.Space))
        {
            dash();
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void DashFromUI() // ฟังก์ชันนี้จะถูกเรียกจาก UI Button
    {
        if (CanDash())
        {
            dash();
        }
    }

    private void dash()
{
    if (animator != null)
    {
        animator.SetTrigger("Dash");
    }

    // ทำการ dash โดยการเคลื่อนที่ Object ไปทางด้านหน้าตามทิศทางที่กำหนดไว้
    Vector3 dashDirection = transform.forward;
    Vector3 dashDestination = transform.position + dashDirection * dashDistance;

    // ให้ใช้ Rigidbody (หากมี) เพื่อเปลี่ยนตำแหน่งของ Object ได้ถูกต้อง
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.MovePosition(dashDestination);
    }
    else
    {
        transform.position = dashDestination;
    }

    isDashing = true;
    currentCooldown = dashCooldown;
    print("dash");
}


    private bool CanDash()
    {
        return currentCooldown <= 0 && !isDashing;
    }
}
