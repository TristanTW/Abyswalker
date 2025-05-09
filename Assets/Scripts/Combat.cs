using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{

    public float attackRange = 1.5f;
    [SerializeField]
    public float lightAttackDamage = 5f;
    [SerializeField]
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
        Vector3 lookDirection = GetComponent<CharacterControll>().LookDirection;
        Vector3 attackOrigin = transform.position + lookDirection;
        Collider[] hitEnemies = Physics.OverlapSphere(attackOrigin, attackRange);


        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                float damage = (type == "Light") ? 10f : 25f;
                enemyHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("Enemy does not have an EnemyHealth component.");
            }
        }

        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Vector3 lookDirection = GetComponent<CharacterControll>().LookDirection;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + lookDirection, 2f);
        }
    }

    // Optional: expose blocking state
    public bool IsBlocking()
    {
        return isBlocking;
    }
}