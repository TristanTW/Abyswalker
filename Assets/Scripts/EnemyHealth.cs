using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider EnemyHealthBar;
    private bool playerNearby = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (EnemyHealthBar != null)
        {
            EnemyHealthBar.maxValue = maxHealth;
            EnemyHealthBar.value = currentHealth;
            EnemyHealthBar.gameObject.SetActive(false); // Hide initially
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (EnemyHealthBar != null)
        {
            EnemyHealthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
        if (EnemyHealthBar != null)
            EnemyHealthBar.gameObject.SetActive(false);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (EnemyHealthBar != null)
                EnemyHealthBar.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (EnemyHealthBar != null)
                EnemyHealthBar.gameObject.SetActive(false);
        }
    }
}

