using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameObject _body;

    private Rigidbody _rb;

    Vector3 _mouseLocation = new Vector3 (0, 0, 1);

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Movement();
        Rotation();
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

    private void Rotation()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(-transform.transform.up, 0);

        if (plane.Raycast(ray, out float distance))
        {
            _mouseLocation = ray.GetPoint(distance);
        }
        _mouseLocation.y = _body.transform.position.y;


        _body.transform.LookAt(_mouseLocation);
    }

    private void Dodge()
    {

    }
}

