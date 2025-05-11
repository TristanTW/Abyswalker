using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private AudioClip _doDamage;

    public GameObject pointOrb;
    private GameObject player;
    private float moveSpeed = 3f;
    private float attackRange = 2f;

    public float maxHealth = 100f;
    private float currentHealth;

    public Transform healthBarCanvas;
    public Image healthFillImage;
    private Camera mainCamera;
    private float targetFill = 1f;

    public float damage = 10f;
    public float damageCooldown = 10f;
    private float damageCooldownTimer;

    private bool playerNearby = false;

    private CharacterControll characterControllerScript;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        damageCooldownTimer = damageCooldown;

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
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
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
        if (damageCooldownTimer >= damageCooldown)
        {
            characterControllerScript.TakeDamage(damage);
            damageCooldownTimer = 0f;
        }
        else
        {
            damageCooldownTimer += Time.deltaTime;
        }
    }
    
    public void TakeDamage(float amount)
    {
        //sound
        AudioControllerScript.Instance.PlaySound(_doDamage);

        //end sound


        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"[TakeDamage] Damage: {amount}, Health: {currentHealth}");

        if (healthFillImage != null)
        {
            targetFill = currentHealth / maxHealth;
        }

        if (currentHealth <= 0.01f)

        {
            Debug.Log("[TakeDamage] Health is zero or less. Calling Die().");
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
        if (healthBarCanvas != null)
            healthBarCanvas.gameObject.SetActive(false);

        Vector2 deathLocation = new Vector2(transform.position.x, transform.position.z);

        Destroy(gameObject);

        Vector3 deathDropSpawnLocation = new Vector3(deathLocation.x, 1, deathLocation.y);

        GameObject deathDrop = Instantiate(pointOrb, deathDropSpawnLocation, Quaternion.identity);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (healthBarCanvas != null)
                healthBarCanvas.gameObject.SetActive(true);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (healthBarCanvas != null)
                healthBarCanvas.gameObject.SetActive(true);

        }
    }
}