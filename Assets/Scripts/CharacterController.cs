using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int forward = 0;
        int backward = 0;
        int left = 0;
        int right = 0;

        if (Input.GetKey(KeyCode.W))
        {
            forward = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            backward = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            left = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right = 1;
        }

        Vector3 directionVector = new Vector3(left + right, 0, forward + backward);

        Vector3 movementVector = directionVector.normalized;

        transform.position = new Vector3(transform.position.x + (movementVector.x * _speed * Time.deltaTime), transform.position.y, transform.position.z + (movementVector.z * _speed * Time.deltaTime));
    }
}
