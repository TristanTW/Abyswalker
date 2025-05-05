using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject enemy;
    public GameObject parent;

    GameObject player;

    Vector3 spawnPosition;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        spawnPosition = parent.transform.position;
        spawnPosition.y = player.transform.position.y;

        GameObject enemyObject = Instantiate(enemy, spawnPosition, Quaternion.identity);
    }
}
