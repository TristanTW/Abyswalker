using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private float moveSpeed = 3f;
    private float attackRange = 2f;

    public float maxHealth = 100f;
    private float currentHealth;

    public Transform healthBarCanvas;
    public Image healthFillImage;
    private Camera mainCamera;
    private float targetFill = 1f;

    private bool playerNearby = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
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
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            if (distance > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
        Debug.Log("Enemy is aan het attacken");
    }

    public void TakeDamage(float amount)
    {
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


        Destroy(gameObject);
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