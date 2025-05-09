//using UnityEngine;

//public class Points : MonoBehaviour
//{
//    private PointsCounterScript pointsUIScript;
//    [SerializeField] private AudioSource _audioSource;
//    [SerializeField] private AudioClip _collectPoints;

//    void Start()
//    {
//        GameObject pointCounter = GameObject.FindWithTag("PointCounter");

//        if (pointCounter != null)
//        {
//            pointsUIScript = pointCounter.GetComponent<PointsCounterScript>();
//            if (pointsUIScript == null)
//            {
//                Debug.LogError("PointsCounterScript not found on the PointCounter object.");
//            }
//        }
//        else
//        {
//            Debug.LogError("PointCounter GameObject not found with the tag 'PointCounter'.");
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("PlayerTrigger"))
//        {
//            if (pointsUIScript != null)
//            {
//                pointsUIScript.AddPoints(1);
//                //sound
                
//                _audioSource.PlayOneShot(_collectPoints);
//                //end sound
//            }

//            Destroy(gameObject);
//        }
//    }
//}