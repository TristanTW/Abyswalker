using UnityEngine;
using System.Collections.Generic;

public class GraveSpawning : MonoBehaviour
{
    public GameObject graveSpawn;
    public GameObject bossGraveSpawn;
    public List<Collider> roomColliders;
    public Collider bossRoomCollider;
    public float radius = 5f;
    public int maxGraveAmount = 3;
    public int maxBossGraveAmount = 2;

    private GameObject player;
    private GameObject boss;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        boss = GameObject.FindWithTag("Boss");
    }

    public void SpawnGravesAround(bool isBoss)
    {
        Collider currentRoom = GetPlayerCurrentRoom();
        if (currentRoom == null)
        {
            Debug.LogWarning("Player is not inside any room.");
            return;
        }

        int graveAmount = 0;
        if (!isBoss) {
            while (graveAmount < maxGraveAmount)
            {
                Vector3? spawnPosition = GetValidSpawnPointOnCircle(radius, currentRoom, player);
                if (spawnPosition != null)
                {
                    Instantiate(graveSpawn, spawnPosition.Value, Quaternion.identity);
                    graveAmount++;
                }
            }
        } else {
            while (graveAmount < maxBossGraveAmount)
            {
                Vector3? spawnPosition = GetValidSpawnPointOnCircle(radius, currentRoom, boss);
                if (spawnPosition != null)
                {
                    Instantiate(graveSpawn, spawnPosition.Value, Quaternion.identity);
                    graveAmount++;
                }
            }
        }
    }

    public void SpawnBossGraveAround()
    {
        Vector3 spawnPosition = bossRoomCollider.bounds.center;
        spawnPosition.y = 0;

        Instantiate(bossGraveSpawn, spawnPosition, Quaternion.identity);
    }

    private Collider GetPlayerCurrentRoom()
    {
        foreach (Collider room in roomColliders)
        {
            if (room != null && room.bounds.Contains(player.transform.position))
            {
                return room;
            }
        }

        return null;
    }

    private Vector3? GetValidSpawnPointOnCircle(float radius, Collider areaCollider, GameObject spawnTarget)
    {
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 offset2D = Random.insideUnitCircle.normalized * radius;
            Vector3 spawnPoint = new Vector3(
                spawnTarget.transform.position.x + offset2D.x,
                0,
                spawnTarget.transform.position.z + offset2D.y
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
