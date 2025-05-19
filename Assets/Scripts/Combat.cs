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

    [SerializeField]
    private float _lightAttackMovementCooldown = 0.1f;
    [SerializeField]
    private float _heavyAttackMovementCooldown = 0.3f;

    [SerializeField] private GameObject _shieldSprite;
    private bool isBlocking = false;
    private bool isAttacking = false;

    private float lastAttackTime = 0f;

    private void Start()
    {
        _shieldSprite.SetActive(false);
    }
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
            _shieldSprite.SetActive(true);
            Debug.Log("Blocking started");
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            isBlocking = false;
            _shieldSprite.SetActive(false);
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
            GetComponent<CharacterControll>().movementCooldown = (int)(_lightAttackMovementCooldown * 100);
        }

        // Heavy attack
        if (Input.GetMouseButtonDown(1) && Time.time - lastAttackTime >= heavyAttackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack("Heavy"));
            GetComponent<CharacterControll>().movementCooldown = (int)(_heavyAttackMovementCooldown * 100);
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
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                float damage = (type == "Light") ? lightAttackDamage : heavyAttackDamage;
                string damageType = (type == "Light") ? "Light" : "Heavy";
                enemyController.TakeDamage(damage, damageType);
            }
            else
            {
                Debug.LogWarning("Enemy does not have an EnemyController component.");
            }
        }

        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }
    void OnDrawGizmos()
    {
        

        // Get the real look direction from the movement script
        CharacterControll movement = GetComponent<CharacterControll>();
        if (movement == null) return;

        Vector3 lookDirection = movement.LookDirection;
        Vector3 attackOrigin = transform.position + lookDirection;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRange);
    }


    // Optional: expose blocking state
    public bool IsBlocking()
    {
        return isBlocking;
    }
}