using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{

    public float attackRange = 1.5f;
    public float lightAttackDamage = 5f;
    public float heavyAttackDamage = 10f;
    public Transform attackPoint; // Assign this in the inspector
    public LayerMask enemyLayers; // Assign this to “Hydra” layer or tag

    [Header("Combat Settings")]
    public float lightAttackCooldown = 0.5f;
    public float heavyAttackCooldown = 1f;

    private bool isBlocking = false;
    private bool isAttacking = false;

    private float lastAttackTime = 0f;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Block toggle (can change to "hold" style if preferred)
        if (Input.GetKeyDown(KeyCode.F))
        {
            isBlocking = true;
            Debug.Log("Blocking started");
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            isBlocking = false;
            Debug.Log("Blocking stopped");
        }

        // Don't allow attacking while blocking
        if (isBlocking || isAttacking)
            return;

        // Light attack
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= lightAttackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack("Light"));
        }

        // Heavy attack
        if (Input.GetMouseButtonDown(1) && Time.time - lastAttackTime >= heavyAttackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack("Heavy"));
        }
    }

    System.Collections.IEnumerator PerformAttack(string type)
    {
        isAttacking = true;

        Debug.Log(type + " attack performed");

        float duration = (type == "Light") ? lightAttackCooldown : heavyAttackCooldown;

        // Attack check - adjust range as needed
        float attackRange = 2f;
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, attackRange);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyHealth EnemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    float damage = (type == "Light") ? 10f : 25f;
                    enemy.TakeDamage(damage);
                }
            }
        }

        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, 2f);
    }
    // Optional: expose blocking state
    public bool IsBlocking()
    {
        return isBlocking;
    }
}