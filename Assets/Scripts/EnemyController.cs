using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private float moveSpeed = 3f;
    private float attackRange = 2f;
    private float rotationSpeed = 10f;

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            if (distance > attackRange)
            {
                // Move toward the player
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Rotate to face movement direction
                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
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