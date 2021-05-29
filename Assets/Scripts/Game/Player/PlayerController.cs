using System;
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

    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float currentSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Transform _cameraMainTransform;

    private bool _isWalking;
    private bool _isRunning;
    private bool _startJump;
    
    private int _groundMask = 6;
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
    }

    void Update()
    {
        _groundedPlayer = _controller.isGrounded;

        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = _cameraMainTransform.forward * move.z + _cameraMainTransform.right * move.x;
        move.y = 0f;

        _controller.Move(move * Time.deltaTime * currentSpeed);

        // Changes the height position of the player..
        if (jumpControl.action.triggered && _groundedPlayer)
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

        //CheckGround();

        UpdateAnimator();
    }


    private void FixedUpdate()
    {
        CheckGround();
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
    }


    private void CheckGround()
    {
        _animator.SetBool("isGrounded", Physics.Raycast(transform.position, Vector3.down, distToGround));
    }
    
    private void CheckGround1()
    {
        //int layerMask = 1 << 8;


        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;

        Debug.Log(Physics.Raycast(transform.position, Vector3.down, distToGround));
        
        
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f, _groundMask))
        {
            Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red);
            Debug.Log("Did Hit");
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);
            Debug.DrawRay(transform.position, Vector3.down * 10f, Color.green);
            Debug.Log("Did not Hit");
            //playerController.isGroundedAnim = true;
        }
        
        
  
    }
}