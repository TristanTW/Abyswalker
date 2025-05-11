using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControll : MonoBehaviour
{

    [SerializeField] private AudioClip _recieveDamage;
    [SerializeField] private GameObject _damageScreen;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameObject _body;

    private float _hitPoints = 100;
    private GameObject _character;
    private GameObject _skeletonSword;

    private float _maxHitPoints = 100;
    [SerializeField] private int _healsRemaining = 5;
    [SerializeField] private TextMeshProUGUI _healCounterText;

    private Rigidbody _rb;
    private Rigidbody _skellybody;

    public Vector3 _lookDirection = new Vector3(1, 0, 0);
    public Vector3 LookDirection => _lookDirection;

    private bool _isDodging = false;
    private Stopwatch _dodgeTimer = new Stopwatch();
    [SerializeField]
    private float _dodgePower = 10;
    [SerializeField]
    private float _dodgeCooldown = 1;
    [SerializeField]
    private bool _rollThroughEnemy = false;

    private Vector3 _mouseLocation = new Vector3(0, 0, 1);


    void Start()
    {
        Time.timeScale = 1.0f;
        _rb = GetComponent<Rigidbody>();
        _skellybody = GetComponent<Rigidbody>();

        UpdateHealCounterUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Healing();
        }


    }
    void FixedUpdate()
    {
        if (!_isDodging)
        {
            Movement();
        }
        Rotation();
        Dodge();

        //lessen damage screen
        if (_damageScreen != null)
        {
            if (_damageScreen.GetComponent<Image>().color.a > 0)
            {
                var color = _damageScreen.GetComponent<Image>().color;
                color.a -= 0.05f;
                _damageScreen.GetComponent<Image>().color = color;
            }
        }
    }
    private void UpdateHealCounterUI()
    {
        _healCounterText.text = _healsRemaining.ToString();
    }
    private void Healing()
    {
        if (_healsRemaining > 0)
        {
            _hitPoints += 15;
            if (_hitPoints > _maxHitPoints)
            {
                _hitPoints = _maxHitPoints;
            }

            _healsRemaining--;
            UpdateHealCounterUI();
        }
        else
        {
            
        }
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
        Plane plane = new Plane(-transform.up, 1);

        if (plane.Raycast(ray, out float distance))
        {
            _mouseLocation = ray.GetPoint(distance);
        }
        _mouseLocation.y = _body.transform.position.y;

        Vector3 direction = _mouseLocation - _body.transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _body.transform.rotation = Quaternion.Slerp(
                _body.transform.rotation,
                targetRotation,
                Time.deltaTime * _rotationSpeed
            );
        }
        _lookDirection = direction.normalized;
    }

    private void Dodge()
    {
        if (Input.GetKey(KeyCode.Space) && _isDodging == false)
        {
            _rb.AddForce(_lookDirection * _dodgePower, ForceMode.Impulse);
            _isDodging = true;
            _dodgeTimer.Restart();
            if (_rollThroughEnemy) GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("Enemy");
        }
        if ((float)_dodgeTimer.ElapsedMilliseconds / 1000 >= _dodgeCooldown)
        {
            _isDodging = false;
            _dodgeTimer.Stop();
            _rb.angularVelocity = Vector3.zero;
            if (_rollThroughEnemy) GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("");
        }
    }


    public float ReturnHealth()
    {
        return _hitPoints;
    }

    public void TakeDamage(float damage)
    {
        _hitPoints -= damage;
        //sound
        AudioControllerScript.Instance.PlaySound(_recieveDamage);
        //end sound

        //show jelly damage screen
        if (_damageScreen != null)
        {

            var color = _damageScreen.GetComponent<Image>().color;
            color.a = 1f;
            _damageScreen.GetComponent<Image>().color = color;
        }
    }
}