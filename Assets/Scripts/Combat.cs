using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    //attacking
    public float attackRange = 1.5f;
    [SerializeField]
    public float lightAttackDamage = 5f;
    [SerializeField]
    public float heavyAttackDamage = 10f;
    public Transform attackPoint; // Assign this in the inspector
    public LayerMask enemyLayers; // Assign this to “Hydra” layer or tag

    public float lightAttackCooldown = 0.5f;
    public float heavyAttackCooldown = 1f;

    [SerializeField]
    private float _lightAttackMovementCooldown = 0.1f;
    [SerializeField]
    private float _heavyAttackMovementCooldown = 0.3f;
    private float lastAttackTime = 0f;

    //shield UI
    [SerializeField] private Image _shieldSpriteFrame;
    [SerializeField] private Image _shieldSpriteCurrentHP;
    [SerializeField] private Image _shieldSpriteBroken;
    [SerializeField] private Image _shieldSpriteRecharging;
    public bool isBlocking = false;
    public bool isAttacking = false;


    //block settings
    public int maxBlockHits = 4;
    private int currentBlockHits;
    [SerializeField]
    public float blockRechargeTime = 10f;
    private bool blockRecharging = false;
    private float rechargerTimerForShieldSprite = 0f;
    private AudioSource audioSource;
    private Animator _playerAnimator;
    public bool _isPositionLocked;
    //block sound
    [SerializeField] private AudioClip blockHitSound;

    //breaksoud
    [SerializeField] private AudioClip blockBreakSound;


    private void Start()
    {
        _isPositionLocked = false;
        _playerAnimator = GetComponent<Animator>();
        _shieldSpriteFrame.enabled = false;
        _shieldSpriteCurrentHP.enabled = false;
        _shieldSpriteBroken.enabled = false;
        _shieldSpriteRecharging.enabled = false;

        currentBlockHits = maxBlockHits;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    void Update()
    {
        HandleInput();
        if (blockRecharging == true)
        {

            rechargerTimerForShieldSprite += Time.deltaTime;
            if (rechargerTimerForShieldSprite >= 1f / 10)
            {
                _shieldSpriteRecharging.fillAmount += 0.1f / 10;
                rechargerTimerForShieldSprite = 0f;
            }
        }

    }

    void HandleInput()
    {
        // Block toggle (can change to "hold" style if preferred)
        if (Input.GetKey(KeyCode.F))
        {
            if (!blockRecharging)
            {
                isBlocking = true;
                _shieldSpriteFrame.enabled = true;
                _shieldSpriteCurrentHP.enabled = true;
                Debug.Log("Blocking started");
                _playerAnimator.SetBool("isBlocking", true);
            }
            else
            {
                Debug.Log("Block is recharging!");
                _playerAnimator.SetBool("isBlocking", false);

            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            isBlocking = false;
            _playerAnimator.SetBool("isBlocking", false);

            _shieldSpriteFrame.enabled = false;
            _shieldSpriteCurrentHP.enabled = false;
            Debug.Log("Blocking stopped");
        }

        // Don't allow attacking while blocking
        if (isBlocking || isAttacking)
            return;

        // Light attack
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= lightAttackCooldown)
        {
            _playerAnimator.SetBool("isWalking", false);
            _playerAnimator.SetBool("isAttacking", true);

            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack("Light"));
            GetComponent<CharacterControll>().movementCooldown = (int)(_lightAttackMovementCooldown * 100);
        }

        // Heavy attack
        if (Input.GetMouseButtonDown(1) && Time.time - lastAttackTime >= heavyAttackCooldown)
        {
            _playerAnimator.SetBool("isWalking", false);
            _playerAnimator.SetBool("isAttacking", true);

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

        }

        yield return new WaitForSeconds(duration);
        _playerAnimator.SetBool("isAttacking", false);
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
    public bool CanBlock()
    {
        return !blockRecharging && currentBlockHits > 0;
    }
    public void RegisterBlockedHit()
    {
        if (!isBlocking) return;

        currentBlockHits--;
        _shieldSpriteCurrentHP.fillAmount -= 0.25f;
        if (blockHitSound != null)
        {
            audioSource.PlayOneShot(blockHitSound);
        }

        Debug.Log("Blocked hit. Remaining: " + currentBlockHits);

        if (currentBlockHits <= 0)
        {
            if (!_isPositionLocked)
            {

                _shieldSpriteBroken.enabled = true;
                _shieldSpriteRecharging.enabled = true;
                _shieldSpriteRecharging.fillAmount = 0;

                if (blockBreakSound != null)
                {
                    audioSource.PlayOneShot(blockBreakSound);
                }

                StartCoroutine(BlockRecharge());
            }


        }
    }


    private System.Collections.IEnumerator BlockRecharge()
    {
        //on floor needs to get up
        _playerAnimator.SetBool("isGuardBroken", true);
        _playerAnimator.SetBool("isBlocking", false);
        _isPositionLocked = true;
        yield return new WaitForSeconds(5);
        _isPositionLocked = false;
        _playerAnimator.SetBool("isGuardBroken", false);
        //stood up

        isBlocking = false;
        _shieldSpriteCurrentHP.enabled = false;
        _shieldSpriteFrame.enabled = false;
        blockRecharging = true;
        Debug.Log("Block depleted. Recharging...");
        yield return new WaitForSeconds(blockRechargeTime);

        blockRecharging = false;
        currentBlockHits = maxBlockHits;


        _shieldSpriteRecharging.fillAmount = 1;
        _shieldSpriteBroken.enabled = false;
        _shieldSpriteRecharging.enabled = false;
        _shieldSpriteCurrentHP.fillAmount = 1;
        Debug.Log("Block recharged.");
    }
}