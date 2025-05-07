using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    private float moveSpeed = 3f;
    private float attackRange = 2f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
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
    }

    void Attack()
    {
        Debug.Log("Enemy is aan het attacken");
    }
}