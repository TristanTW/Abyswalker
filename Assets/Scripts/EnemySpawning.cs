using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject enemy;

    GameObject player;

    Vector3 spawnPosition;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        spawnPosition = transform.parent.position;
        spawnPosition.y = player.transform.position.y;

        GameObject enemyObject = Instantiate(enemy, spawnPosition, Quaternion.identity);
    }
}
