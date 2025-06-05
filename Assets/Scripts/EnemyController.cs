using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private AudioClip _doDamage;

    private Combat combatScript;

    public GameObject _body;
    public Rigidbody _rb;
    public GameObject pointOrb;
    public bool isBoss;

    private bool isRecharging = false;
    private float rechargeDuration = 2f;

    public Transform healthBarCanvas;
    public Image healthFillImage;
    private Camera mainCamera;
    private float targetFill = 1f;

    public float maxHealth = 100f;
    private float currentHealth;

    private GameObject player;
    private float moveSpeed = 3f;
    private float attackRange = 2f;

    public float damage = 10f;
    public float damageCooldown = 10f;
    private float damageCooldownTimer = 0f;
    [SerializeField] private Image _swordVisueleCooldown;

    private CharacterControll characterControllerScript;
    private GraveSpawning graveSpawningScript;

    public Material defaultMat;
    public Material hitMat;
    public float resetMaterialDelay = 0.2f;

    private float _knockbackPowerLight = 2;
    private float _knockbackPowerHeavy = 4;

    private bool hasSpawnedAds = false;

    private Animator _enemyAnimator;
    private bool isdead = false;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");

        combatScript = player.GetComponent<Combat>();
        if (combatScript == null)
        {
            Debug.LogError("Combat script not found on the Player object.");
        }

        graveSpawningScript = player.GetComponent<GraveSpawning>();
        if (graveSpawningScript == null)
        {
            Debug.LogError("GraveSpawning script not found on the Player object.");
        }
    }

    void Start()
    {
        damageCooldownTimer = damageCooldown;
        _enemyAnimator = GetComponent<Animator>();
        if (player != null)
        {
            characterControllerScript = player.GetComponent<CharacterControll>();
            if (characterControllerScript == null)
            {
                Debug.LogError("CharacterControllerScript not found on the Player object.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found with the tag 'Player'.");
        }

        mainCamera = Camera.main;
        currentHealth = maxHealth;
        healthBarCanvas.gameObject.SetActive(true);
        if (healthFillImage != null)
        {
            healthFillImage.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void Update()
    {
        if (isdead) { return; }

        this.gameObject.transform.LookAt(player.transform.position);
        if (player != null && !isRecharging)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                _swordVisueleCooldown.color = Color.green;
            }
            else
            {
                _swordVisueleCooldown.color = Color.yellow;
                Attack();
            }
        }

        if (healthBarCanvas != null)
        {
            healthBarCanvas.LookAt(mainCamera.transform);
        }

        if (healthFillImage != null)
        {
            Vector3 currentScale = healthFillImage.rectTransform.localScale;
            float smoothFill = Mathf.Lerp(currentScale.x, targetFill, Time.deltaTime * 10f);
            healthFillImage.rectTransform.localScale = new Vector3(smoothFill, 1f, 1f);
        }
    }

    void Attack()
    {
        Debug.Log(damageCooldownTimer);
        if (damageCooldownTimer >= damageCooldown)
        {
            _swordVisueleCooldown.color = Color.red;
            _enemyAnimator.SetBool("_isPunching", true);

            if (combatScript != null && combatScript.IsBlocking())
            {
                Debug.Log("Player Blocked");
                combatScript.RegisterBlockedHit();
            }
            else
            {
                characterControllerScript.TakeDamage(damage);
                Debug.Log("Player took damage");

            }
            damageCooldownTimer = 0f;
        }
        else
        {
            damageCooldownTimer += Time.time;
        }
        StartCoroutine(Recharge());
    }

    public void TakeDamage(float amount, string type)
    {
        if (isdead) { return; }

        //anim
        _enemyAnimator.SetBool("_gotHit", true);

        //sound
        AudioControllerScript.Instance.PlaySound(_doDamage);

        //end sound

        // Change material to indicate damage
        _body.GetComponent<Renderer>().material = hitMat;

        // Start coroutine to revert back
        StartCoroutine(ResetMaterialAfterDelay());

        if (type == "Light")
        {
            _rb.AddForce(characterControllerScript._lookDirection * _knockbackPowerLight, ForceMode.Impulse);
        }
        else if (type == "Heavy")
        {
            _rb.AddForce(characterControllerScript._lookDirection * _knockbackPowerHeavy, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("[EnemyController] Wrong take damage type input");
        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"[TakeDamage] Damage: {amount}, Health: {currentHealth}");

        if (healthFillImage != null)
        {
            targetFill = currentHealth / maxHealth;
        }

        if (isBoss == true && hasSpawnedAds == false && currentHealth <= maxHealth / 2)
        {
            graveSpawningScript.SpawnGravesAround(true, gameObject);
            hasSpawnedAds = true;
        }

        if (currentHealth <= 0.01f)

        {
            Debug.Log("[TakeDamage] Health is zero or less. Calling Die().");
            Die();
        }
    }

    private System.Collections.IEnumerator ResetMaterialAfterDelay()
    {
        yield return new WaitForSeconds(resetMaterialDelay);
        _body.GetComponent<Renderer>().material = defaultMat;
        _enemyAnimator.SetBool("_gotHit", false);
    }

    void Die()
    {
        _enemyAnimator.SetBool("_isDead", true);
        Debug.Log("Enemy defeated!");
        if (healthBarCanvas != null)
            healthBarCanvas.gameObject.SetActive(false);

        Vector2 deathLocation = new Vector2(transform.position.x, transform.position.z);

        isdead = true;
        StartCoroutine(WaitForDeath());

        Vector3 deathDropSpawnLocation = new Vector3(deathLocation.x, 1, deathLocation.y);

        GameObject deathDrop = Instantiate(pointOrb, deathDropSpawnLocation, Quaternion.identity);
    }

    private System.Collections.IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(1.5f);
        _enemyAnimator.SetBool("_isDead", false);
        Destroy(gameObject);


    }
    private System.Collections.IEnumerator Recharge()
    {
        isRecharging = true;
        yield return new WaitForSeconds(rechargeDuration);
        _enemyAnimator.SetBool("_isPunching", false);
        isRecharging = false;

    }
}
