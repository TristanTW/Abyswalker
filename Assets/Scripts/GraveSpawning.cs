//using UnityEngine;

//public class GraveSpawning : MonoBehaviour
//{
//    public GameObject graveSpawn;
//    public Collider roomCollider;
//    public float radius;
//    public int maxGraveAmount;

//    public EnemySpawning enemySpawner; // Reference to the EnemySpawning script

//    GameObject player;
//    int graveAmount;

//    private void Awake()
//    {
//        player = GameObject.FindWithTag("Player");
//    }

//    public void StartSpawning()
//    {
//        // Spawn graves only if the graveAmount is less than the max
//        while (graveAmount < maxGraveAmount)
//        {
//            var spawnPosition = GetValidSpawnPointOnCircle(radius, roomCollider);
//            if (spawnPosition != null)
//            {
//                graveAmount++;
//                GameObject graveObject = Instantiate(graveSpawn, spawnPosition.Value, Quaternion.identity);

//                // Spawn an enemy at the grave's position using the EnemySpawning script
//                if (enemySpawner != null)
//                {
//                    enemySpawner.parent = graveObject;  // Set the grave as the parent for the enemy
//                    enemySpawner.Spawn();  // Spawn the enemy at this position
//                }
//                else
//                {
//                    Debug.LogWarning("EnemySpawning component is not assigned!");
//                }
//            }
//            else
//            {
//                break; // Exit if no valid position found
//            }
//        }
//    }

//    private Vector3? GetValidSpawnPointOnCircle(float radius, Collider areaCollider)
//    {
//        int maxAttempts = 10;
//        for (int i = 0; i < maxAttempts; i++)
//        {
//            Vector2 offset2D = Random.insideUnitCircle.normalized * radius;
//            Vector3 spawnPoint = new Vector3(
//                player.transform.position.x + offset2D.x,
//                0,
//                player.transform.position.z + offset2D.y
//            );

//            if (IsPointInsideCollider(areaCollider, spawnPoint))
//            {
//                return spawnPoint;
//            }
//        }

//        return null; // Return null if no valid position found
//    }

//    private bool IsPointInsideCollider(Collider col, Vector3 point)
//    {
//        return col.bounds.Contains(point);
//    }
//}
