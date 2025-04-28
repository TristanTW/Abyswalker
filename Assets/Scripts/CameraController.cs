using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private Camera _followerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera.transform.position = _followerCamera.transform.position;
        _mainCamera.transform.rotation = _followerCamera.transform.rotation;

        _followerCamera.enabled = false;
        _mainCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _followerCamera.transform.position - _mainCamera.transform.position;
        _mainCamera.transform.position += direction * _speed * Time.deltaTime;

        _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, _followerCamera.transform.rotation, _speed * Time.deltaTime);

        //_mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _followerCamera.transform.position, _speed);
    }
}
