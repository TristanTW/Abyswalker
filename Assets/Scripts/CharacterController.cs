using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControll : MonoBehaviour
{

    [SerializeField] private AudioClip _recieveDamage;
    [SerializeField] private AudioClip _healSoundClip;

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

    private float _maxHitPoints = 100;
    [SerializeField] private int _healsRemaining = 5;
    [SerializeField] private TextMeshProUGUI _healCounterText;

    private Rigidbody _rb;

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

    public int movementCooldown = 0;

    public bool canDodge = true;

    public Material defaultMat;
    public Material hitMat;
    public float resetMaterialDelay = 0.2f;

    private Combat _combat;
    private Animator _playerAnimator;
    void Start()
    {
        _combat = GetComponent<Combat>();
        _playerAnimator = GetComponent<Animator>();

        Time.timeScale = 1.0f;
        _rb = GetComponent<Rigidbody>();

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
        if (!_isDodging && movementCooldown <= 0)
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

        if (movementCooldown > 0)
        {
            movementCooldown--;
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
            _hitPoints += 25;
            AudioControllerScript.Instance.PlaySound(_healSoundClip);

            if (_hitPoints > _maxHitPoints)
            {
                _hitPoints = _maxHitPoints;
            }

            _healsRemaining--;
            UpdateHealCounterUI();
        }
        
    }
    private Vector3 GetMovementInput()
    {
        int forward = 0;
        int backward = 0;
        int left = 0;
        int right = 0;

        if (Input.GetKey(KeyCode.W)) forward = 1;
        if (Input.GetKey(KeyCode.S)) backward = -1;
        if (Input.GetKey(KeyCode.A)) left = -1;
        if (Input.GetKey(KeyCode.D)) right = 1;

        return new Vector3(left + right, 0, forward + backward);
    }
    private void Movement()
    {
        Vector3 directionVector = GetMovementInput();

        float effectiveSpeed = _speed;
        _playerAnimator.SetBool("isWalking", true);
        if (_combat != null && _combat.IsBlocking())
        {
            _playerAnimator.SetBool("isBlocking", true);
            _playerAnimator.SetBool("isWalking", false);

            effectiveSpeed *= 0.2f; // 20% speed when blocking
        }

        Vector3 movementVector = directionVector.normalized * effectiveSpeed * Time.fixedDeltaTime;

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
        if (Input.GetKey(KeyCode.Space) && !_isDodging)
        {
            Vector3 movementInput = GetMovementInput();

            if (movementInput != Vector3.zero)
            {
                float effectiveDodgePower = _dodgePower;

                if (_combat != null && _combat.IsBlocking())
                {
                    effectiveDodgePower *= 0.2f; // Optional: slow dodge when blocking
                }

                _rb.AddForce(movementInput.normalized * effectiveDodgePower, ForceMode.Impulse);
            }

            _isDodging = true;
            _dodgeTimer.Restart();

            if (_rollThroughEnemy)
            {
                GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("Enemy");
            }

            canDodge = false;
        }

        if ((float)_dodgeTimer.ElapsedMilliseconds / 1000 >= _dodgeCooldown)
        {
            _isDodging = false;
            _dodgeTimer.Stop();
            _rb.angularVelocity = Vector3.zero;

            if (_rollThroughEnemy)
            {
                GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("");
            }

            canDodge = true;
        }
    }


    public float ReturnHealth()
    {
        return _hitPoints;
    }

    public void TakeDamage(float damage)
    {
        _hitPoints -= damage;

        // Sound
        AudioControllerScript.Instance.PlaySound(_recieveDamage);

        // Change material to indicate damage
        _body.GetComponent<Renderer>().material = hitMat;

        // Start coroutine to revert back
        StartCoroutine(ResetMaterialAfterDelay());

        // Show jelly damage screen
        if (_damageScreen != null)
        {
            var color = _damageScreen.GetComponent<Image>().color;
            color.a = 1f;
            _damageScreen.GetComponent<Image>().color = color;
        }
    }

    private System.Collections.IEnumerator ResetMaterialAfterDelay()
    {
        yield return new WaitForSeconds(resetMaterialDelay);
        _body.GetComponent<Renderer>().material = defaultMat;
    }
}