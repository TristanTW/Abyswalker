using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private float moveSpeed = 3f;
    private float attackRange = 2f;

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            float distance = Vector3.Distance(transform.position, targetPosition);

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