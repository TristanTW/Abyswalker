using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private Camera _followerCamera;

    [SerializeField]
    private float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        _mainCamera.transform.position = _followerCamera.transform.position;
        _mainCamera.transform.rotation = _followerCamera.transform.rotation;

        _followerCamera.enabled = false;
        _mainCamera.enabled = true;
    }

    void Update()
    {
        // Vector3 direction = _followerCamera.transform.position - _mainCamera.transform.position;
        // _mainCamera.transform.position += direction * _speed * Time.deltaTime;

        _mainCamera.transform.position = Vector3.SmoothDamp(_mainCamera.transform.position, _followerCamera.transform.position, ref velocity, smoothTime);

        // _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, _followerCamera.transform.rotation, _speed * Time.deltaTime);

        //_mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _followerCamera.transform.position, _speed);
    }
}
