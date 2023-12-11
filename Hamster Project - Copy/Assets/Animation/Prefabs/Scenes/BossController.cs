using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject spellPrefab;
    public GameObject castNoPrefab;
    public GameObject spellNoPrefab;
    public Transform Position1;
    public Transform Position2;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float timer = 5f;
    private bool DefaultPos = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TriggerRandomAnimation();
            timer = 2f; // Reset the timer
        }
    }

    void TriggerRandomAnimation()
    {
        // Define an array of trigger names
        string[] animationTriggers = { "walking", "attack", "spell", "cast", "spellno", "castno", "attackno" };
        //string[] animationTriggers = {"spellno"};

        // Get a random index
        int randomIndex = Random.Range(0, animationTriggers.Length);

        // Set the trigger based on the random index
        animator.SetTrigger(animationTriggers[randomIndex]);

        if (animationTriggers[randomIndex] == "cast")
        {
            Debug.Log("Inside Cast Animation");
            if (DefaultPos)
            {
                Debug.Log("At default");
                Vector2 newPosition = new Vector2(294, 104);
                Debug.Log("New position: " + newPosition);
                transform.parent.position = newPosition; // Move the parent GameObject
                FlipSprite();
                DefaultPos = false;
            }
            else
            {
                Debug.Log("Not default");
                Vector2 newPosition = new Vector2(342, 104);
                Debug.Log("New position: " + newPosition);
                transform.parent.position = newPosition; // Move the parent GameObject
                FlipSprite();
                DefaultPos = true;
            }
        }



        else if (animationTriggers[randomIndex] == "spell")
        {
            StartCoroutine(SpellAnimationSequence());
        }
        else if (animationTriggers[randomIndex] == "walking")
        {
            StartCoroutine(WalkingAnimation());
        }
        else if (animationTriggers[randomIndex] == "attack")
        {
            StartCoroutine(AttackAnimation());
        }
        else if (animationTriggers[randomIndex] == "spellno")
        {
            StartCoroutine(SpellNoAnimation());
        }
        else if (animationTriggers[randomIndex] == "castno")
        {
            StartCoroutine(CastNoAnimation());
        }
        else if (animationTriggers[randomIndex] == "attackno")
        {
            StartCoroutine(AttackNoAnimation());
        }
    }

    IEnumerator SpellAnimationSequence()
    {
        // Change the boss sprite color to red
        spriteRenderer.color = Color.red;

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Instantiate the spell prefab at the desired position
        if (DefaultPos)
        {
            GameObject spellInstance = Instantiate(spellPrefab, transform.position + new Vector3(-15f, 4f, 0f), Quaternion.identity);
            // Destroy the instantiated prefab after 1 second
            Destroy(spellInstance, 1f);
        }

        else
        {
            GameObject spellInstance = Instantiate(spellPrefab, transform.position + new Vector3(15f, 4f, 0f), Quaternion.identity);
            // Destroy the instantiated prefab after 1 second
            Destroy(spellInstance, 1f);
        }

        // Change the boss sprite color back to its original color (modify as needed)
        spriteRenderer.color = Color.white;
    }

    IEnumerator WalkingAnimation()
    {
        // Move back and forth for 2 seconds
        float elapsedTime = 0f;
        float duration = 2f;
        float walkRange = 5f; // Adjust the walk range as needed
        float maxXLimit = 320f; // Set the maximum X limit
        Vector2 originalPosition = transform.position;
        float direction = DefaultPos ? -1f : 1f; // Determine the direction based on DefaultPos

        while (elapsedTime < duration)
        {
            // Calculate the next position
            Vector2 nextPosition = originalPosition + new Vector2(direction * Mathf.PingPong(elapsedTime * 2f, walkRange), 0f);

            // Limit the X position to stay within the maxXLimit
            nextPosition.x = Mathf.Clamp(nextPosition.x, originalPosition.x - maxXLimit, originalPosition.x + maxXLimit);

            // Update the boss position
            transform.position = nextPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset position
        transform.position = originalPosition;
    }




    IEnumerator AttackAnimation()
    {
        // Move a little bit higher for 1 second
        float originalY = transform.position.y;
        transform.position = new Vector2(transform.position.x, originalY + 2f);
        yield return new WaitForSeconds(1f);
        transform.position = new Vector2(transform.position.x, originalY);
    }

    IEnumerator SpellNoAnimation()
    {
        Vector3 spellNoPosition;

        // Rotate spellNoPrefab by 90 degrees
        if (DefaultPos)
        {
            // If DefaultPos is true, move more left (adjust the X coordinate as needed)
            spellNoPosition = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
        }
        else
        {
            spellNoPosition = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
        }

        GameObject spellNoInstance = Instantiate(spellNoPrefab, spellNoPosition, Quaternion.Euler(0f, DefaultPos ? 180f : 0f, 90f));
        yield return new WaitForSeconds(1f);
        Destroy(spellNoInstance);
    }

    IEnumerator CastNoAnimation()
    {
        // Instantiate a prefab similar to spell instance
        GameObject castNoInstance = Instantiate(castNoPrefab, new Vector2(transform.position.x, transform.position.y + 5f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(castNoInstance);
    }

    IEnumerator AttackNoAnimation()
    {
        // Move a little bit higher for 1 second
        float originalY = transform.position.y;
        transform.position = new Vector2(transform.position.x, originalY + 2f);
        yield return new WaitForSeconds(1f);
        transform.position = new Vector2(transform.position.x, originalY);
    }

    public void FlipSprite()
    {
        // Flip the sprite horizontally
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
