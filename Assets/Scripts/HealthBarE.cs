using UnityEngine;
using UnityEngine.UI;

public class HealthBarE : MonoBehaviour
{
    public Transform target; // The "thing" to follow
    public Vector3 offset = new Vector3(0, 2, 0); // Offset above the head
    public Slider slider;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + offset);
            transform.position = screenPos;
        }
    }

    public void SetHealth(float healthPercent)
    {
        slider.value = healthPercent;
    }
}
