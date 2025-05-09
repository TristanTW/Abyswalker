using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject enemy;
    public GameObject parent;

    GameObject player;

    public void Spawn()
    {
        if (enemy == null || parent == null)
        {
            Debug.LogWarning("Enemy or Parent not assigned.");
            return;
        }

        player = GameObject.FindWithTag("Player");

        Vector3 spawnPosition = parent.transform.position;
        spawnPosition.y = player.transform.position.y;

        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }
}