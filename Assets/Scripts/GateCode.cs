using Unity.VisualScripting;
using UnityEngine;

public class GateCode : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftGate;
    [SerializeField]
    private GameObject _rightGate;
    [SerializeField]
    private GameObject _topBeam;
    [SerializeField]
    private GameObject _bottomBeam;

    [SerializeField]
    private float _gateSpeed;

    private bool _isOpen = true;
    [SerializeField]
    private bool _gateOpenTest = true;

    private Vector3 _leftGateClosed;
    private Vector3 _rightGateClosed;
    private Vector3 _topBeamClosed;
    private Vector3 _bottomBeamClosed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _leftGateClosed = _leftGate.transform.position;
        _rightGateClosed = _rightGate.transform.position;
        _topBeamClosed = _topBeam.transform.position;
        _bottomBeamClosed = _bottomBeam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOpen)
        {
            MoveGatePart(_leftGate, _leftGateClosed, (float)1.5);
            MoveGatePart(_rightGate, _rightGateClosed, (float)-1.5);
            MoveGatePart(_topBeam, _topBeamClosed, -4);
            MoveGatePart(_bottomBeam, _bottomBeamClosed, 4);
        }

        if (!_isOpen)
        {
            MoveGatePart(_leftGate, _leftGateClosed, 0);
            MoveGatePart(_rightGate, _rightGateClosed, 0);
            MoveGatePart(_topBeam, _topBeamClosed, 0);
            MoveGatePart(_bottomBeam, _bottomBeamClosed, 0);
        }
        _isOpen = _gateOpenTest;
    }

    private void MoveGatePart(GameObject gatePart, Vector3 closedPoint, float distance)
    {
        if (gatePart.transform.position.z != closedPoint.z + distance)
        {
            gatePart.transform.position = Vector3.Lerp(gatePart.transform.position, new Vector3(closedPoint.x, closedPoint.y, closedPoint.z + distance), _gateSpeed);
        }
    }
}
