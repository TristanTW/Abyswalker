using UnityEngine;
using UnityEngine.UI;

public class PointsCounterScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Text _pointsCounter;
    [SerializeField] private float _currentPoints;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _pointsCounter.text = _currentPoints.ToString();
    }
}
