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
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
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