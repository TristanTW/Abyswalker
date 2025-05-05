using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    [SerializeField]
    private float _speed;
<<<<<<< Updated upstream
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameObject _body;
=======
    private float _hitPoints = 100;
    private GameObject _character;
    private GameObject _skeletonSword;
>>>>>>> Stashed changes

    private Rigidbody _rb;
    private Rigidbody _skellybody;

    Vector3 _mouseLocation = new Vector3 (0, 0, 1);

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _skellybody = GetComponent<Rigidbody>();


    }

    void FixedUpdate()
    {
        Movement();
        //Rotation();
        Dodge();
    }

    private void Movement()
    {
        int forward = 0;
        int backward = 0;
        int left = 0;
        int right = 0;

        if (Input.GetKey(KeyCode.W)) forward = 1;
        if (Input.GetKey(KeyCode.S)) backward = -1;
        if (Input.GetKey(KeyCode.A)) left = -1;
        if (Input.GetKey(KeyCode.D)) right = 1;

        Vector3 directionVector = new Vector3(left + right, 0, forward + backward);
        Vector3 movementVector = directionVector.normalized * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + movementVector);


    }

<<<<<<< Updated upstream
    private void Rotation()
    {
        //Vector3 mousePos = Input.mousePosition;

        //Vector3 position = new Vector3((mousePos.x) / Screen.width, (mousePos.y) / Screen.height, mousePos.z);

        //Vector3 mouseLocation = _camera.ViewportToWorldPoint(position);
        //mouseLocation.y = 0;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(-transform.transform.up, 0);

        if (plane.Raycast(ray, out float distance))
        {
            _mouseLocation = ray.GetPoint(distance);
            _mouseLocation.Normalize();
        }

        Vector3 lookVector = _mouseLocation - transform.position;
        lookVector.y = 0;
        lookVector.Normalize();

        Vector3 forward = _camera.transform.forward;
        forward.y = 0;
        forward = forward.normalized;

        float dot = Vector2.Dot(new Vector2(forward.x, forward.z), new Vector2(lookVector.x, lookVector.z));
        float rotation = (float)Math.Acos(dot);
        float sin = (float)Math.Asin(dot);

        _body.transform.rotation = new Quaternion(transform.rotation.x, rotation, transform.rotation.z, transform.rotation.w);
        Debug.Log(rotation);
    }

    private void Dodge()
    {

    }
=======
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject == _skeletonSword)
        {
            Debug.Log("hit");
            _hitPoints -= 5;

        }
    }

>>>>>>> Stashed changes
}

