using System.Linq;
using UnityEngine;

public class FinalGateScript : MonoBehaviour
{
    [SerializeField] private GameObject _vicScreen;
    private bool _isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Count() == 0)
        {
            Debug.Log("Putting door active");
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (_isActive == true)
        {
            Debug.Log("wrong tag");
            if (other.gameObject.tag == "PlayerTrigger")
            {
                Debug.Log("VICTORY");
                _vicScreen.SetActive(true);
            }

        }
        Debug.Log("active = false");
    }
}
