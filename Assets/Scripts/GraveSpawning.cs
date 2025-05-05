using UnityEngine;

public class GraveSpawning : MonoBehaviour
{
    public GameObject graveSpawn;
    public Collider roomCollider;
    public float radius;
    public int maxGraveAmount;

    GameObject player;

    Vector3 colliderSize;

    int graveAmount;

    private void Awake()
    {
        Transform colliderTransform = roomCollider.GetComponent<Transform>();

        colliderSize.x = colliderTransform.localScale.x;
        colliderSize.z = colliderTransform.localScale.z;

        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        SpawnGravesAround();
    }

    private void SpawnGravesAround()
    {
        if (graveAmount < maxGraveAmount)
        {
            var spawnPosition = GetValidSpawnPointOnCircle(radius, roomCollider);
            if (spawnPosition != null)
            {
                graveAmount++;
                GameObject graveObject = Instantiate(graveSpawn, spawnPosition.Value, Quaternion.identity);
            }
        }
    }

    private Vector3? GetValidSpawnPointOnCircle(float radius, Collider areaCollider)
    {
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 offset2D = Random.insideUnitCircle.normalized * radius;
            Vector3 spawnPoint = new Vector3(
                player.transform.position.x + offset2D.x,
                0,
                player.transform.position.z + offset2D.y
            );

            if (IsPointInsideCollider(areaCollider, spawnPoint))
            {
                return spawnPoint;
            }
        }

        return null;
    }

    private bool IsPointInsideCollider(Collider col, Vector3 point)
    {
        return col.bounds.Contains(point);
    }

}
