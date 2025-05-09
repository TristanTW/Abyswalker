using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public GraveSpawning graveSpawner;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            graveSpawner.StartSpawning();
            triggered = true;
        }
    }
   
}