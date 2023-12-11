using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    private float currentCooldown;
    [SerializeField] private Animator animator;
    [SerializeField] private Button attackButton;
    [SerializeField] private int damageAmount = 1; 

    private void Start()
    {
        currentCooldown = 0f;

        if (attackButton != null)
        {
            attackButton.onClick.AddListener(Attack);
        }
    }

    private void Update()
    {
        
        if (CanAttack() && Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        
        if (currentCooldown <= 0)
        {
            
            PerformAttack();

            
            currentCooldown = attackCooldown;
        }
    }

    private void PerformAttack()
    {
        
        if (animator != null)
        {
            
            animator.SetTrigger("Attack");
        }

        
        //DealDamageToEnemy();
        print("Player attacking!");
    }

    /*private void DealDamageToEnemy()
    {
        
        GameObject enemy = FindClosestEnemy();
        if (enemy != null)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
*/
    private GameObject FindClosestEnemy()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    private bool CanAttack()
    {
        
        return currentCooldown <= 0;
    }
}
