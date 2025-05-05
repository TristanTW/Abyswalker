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
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
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