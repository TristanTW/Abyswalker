using UnityEngine;

public class Points : MonoBehaviour
{
    private PointsCounterScript pointsUIScript;

    void Start()
    {
        GameObject pointCounter = GameObject.FindWithTag("PointCounter");

        if (pointCounter != null)
        {
            pointsUIScript = pointCounter.GetComponent<PointsCounterScript>();
            if (pointsUIScript == null)
            {
                Debug.LogError("PointsCounterScript not found on the PointCounter object.");
            }
        }
        else
        {
            Debug.LogError("PointCounter GameObject not found with the tag 'PointCounter'.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (pointsUIScript != null)
        {
            pointsUIScript.AddPoints(1);
        }

        Destroy(gameObject);
    }
}