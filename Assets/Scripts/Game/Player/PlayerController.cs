using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GameObject))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject skin;

    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;

    [SerializeField] private InputActionReference runControl;
    //[SerializeField] private InputActionReference attackControl;

    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float currentSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;

    //attack
    public Transform attackPoint;
    [SerializeField] private float timeBetweenAttacks = 0.8f;
    [SerializeField] private float hitEnemiesDelay = 0.5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int playerDamage = 50;


    public LayerMask whatIsEnemy;

    //health
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;

    //healing
    [SerializeField] private int timeBeforeHeal = 5;
    private int _currentTimeBeforeHeal;
    private bool _canHeal;
    
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Transform _cameraMainTransform;

    private bool _isWalking;
    private bool _isRunning;
    private bool _isAttacking;
    private bool _isDefending;
    private bool _startJump;

    //private int _groundMask = 6;
    public float distToGround = 0.95f;

    private Animator _animator;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        runControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        runControl.action.Disable();
    }

    private void Awake()
    {
        _animator = skin.GetComponent<Animator>();
    }

    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        _cameraMainTransform = Camera.main.transform;

        //health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);

        //healing
        StartCoroutine(nameof(HealCoroutine));
    }

    void Update()
    {
        _groundedPlayer = _controller.isGrounded;

        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        if (_isAttacking || _isDefending) movement = Vector2.zero;

        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = _cameraMainTransform.forward * move.z + _cameraMainTransform.right * move.x;
        move.y = 0f;

        _controller.Move(move * Time.deltaTime * currentSpeed);

        // Changes the height position of the player.
        //_groundedPlayer
        if (jumpControl.action.triggered && IsGrounded())
        {
            _startJump = true;
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        else
        {
            _startJump = false;
        }

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg +
                                _cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            CheckWalkAndRun();
        }
        else
        {
            _isRunning = false;
            _isWalking = false;
        }

        CheckAttack();

        CheckDefense();

        UpdateAnimator();
    }

    //Animations and run
    private void CheckWalkAndRun()
    {
        if (!_isWalking)
        {
            currentSpeed = walkSpeed;
            _isWalking = true;
            _isRunning = false;
        }

        if (!Keyboard.current.leftShiftKey.isPressed) return;
        currentSpeed = runSpeed;
        _isWalking = false;
        _isRunning = true;
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isRunning", _isRunning);
        _animator.SetBool("startJump", _startJump);
        _animator.SetBool("isGrounded", IsGrounded());
        _animator.SetBool("isDefending", _isDefending);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        _currentTimeBeforeHeal = timeBeforeHeal;
    }

    IEnumerator HealCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _currentTimeBeforeHeal--;
            if (_currentTimeBeforeHeal <= 0)
            {
                _canHeal = true;
                if (_currentTimeBeforeHeal == 0)
                {
                    PasiveHealing();
                }
            }
            else
            {
                _canHeal = false;
            }
        }
    }

    private void PasiveHealing()
    {
        if (_canHeal && currentHealth < maxHealth)
        {
            currentHealth++;
            healthBar.SetHealth(currentHealth);
            Invoke(nameof(PasiveHealing), 0.1f);
        }
    }

    private void CheckAttack()
    {
        if (Mouse.current.leftButton.isPressed && !_isAttacking && !_isDefending)
        {
            Debug.Log("Attack true");
            _animator.SetBool("isSlashing", true);
            _isAttacking = true;

            StartCoroutine(nameof(AttackCd));
            StartCoroutine(nameof(DamageEnemies));
        }
    }

    IEnumerator DamageEnemies()
    {
        yield return new WaitForSeconds(hitEnemiesDelay);

        //Si le pegamos denbtro del rango dañara a todos los enemigos en el rango
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, whatIsEnemy);
        foreach (Collider enemy in enemies)
        {
            Debug.Log("hit" + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(playerDamage);
        }
    }

    IEnumerator AttackCd()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        _animator.SetBool("isSlashing", false);
        _isAttacking = false;
    }

    private void CheckDefense()
    {
        if (!Keyboard.current.qKey.isPressed)
        {
            _isDefending = false;
        }
        else
        {
            _isDefending = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    /*IEnumerator Cast()
    {
        canCast = false;
        float curTimeLeft=dashDuration;
        while (curTimeLeft >0)
        {
            Vector3 dasher = transform.forward * dashSpeed * Time.deltaTime;
            controls.cc.Move(dasher);
            curTimeLeft -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        controls.canMove = true;
        yield return new WaitForSeconds(cooldown-dashDuration);
        canCast = true;
    }*/
}