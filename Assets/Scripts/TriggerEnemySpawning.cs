using UnityEngine;

public class TriggerEnemySpawning : MonoBehaviour
{
    public GraveSpawning graveSpawning;
    public bool isBossTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            if (!isBossTrigger)
            {
                graveSpawning.SpawnGravesAround(false);
                Destroy(gameObject);
            } else
            {
                graveSpawning.SpawnBossGraveAround(false);
                Destroy(gameObject);
            }
        }
    }
}
