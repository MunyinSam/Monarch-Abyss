using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("IFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("UI")]
    [SerializeField] private Slider healthSlider; // Reference to the health slider

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        // Initialize the health slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = startingHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float _damage)
    {
        // Calculate the amount of health to decrease (25% of startingHealth)
        float damageToApply = 1f / startingHealth;

        // Clamp is upper and lower bound
        currentHealth = Mathf.Clamp(currentHealth - damageToApply, 0, startingHealth);

        // Update the health slider
        UpdateHealthSlider();

        if (currentHealth > 0)
        {
            //anim.SetTrigger("hurt");
            //iframes
            StartCoroutine(Invunerability());
        }
        else
        {
            //anim.SetTrigger("Playerdeath");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        // invunerability duration
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white; // can use this instead of creating new color
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
    private void UpdateHealthSlider()
    {
        // Update the value of the health slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}


