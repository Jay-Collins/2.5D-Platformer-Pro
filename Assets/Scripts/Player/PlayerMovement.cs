using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    [SerializeField] private float _wallJumpControlDelay;
    [SerializeField] private float _wallJumpPush;
    [SerializeField] private float _boxPushPower;

    [Header("References")] 
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private MeshRenderer _meshRenderer;

    public static int orbs;
    public static int lives;
    
    private Transform _parentObject;
    private Vector3 _velocity;

    private float _horizontalInput;
    private float _horizontalMove;
    private float _yVelocity;
    private float _collisionAngle;
    private float _angle;
    private bool _doubleJumped;
    private bool _dead;
    private bool _canWallJump;
    private bool _useJumpVelocityLeft;
    private bool _useJumpVelocityRight;

    private void OnEnable()
    {
        lives = 3;
        _horizontalMove = _horizontalInput;
        
        //subscriptions
        Collectables.orbCollected += OrbsCollected;
        InputManager.jumpStarted += Jump;
        InputManager.movement += CalculateHorizontalInput;

        //reference null checks
        if (_characterController is null)
            Debug.Log("CharacterController is NULL");
    }

    private void Update()
    {
        CalculateVelocity();
        
        if (_dead && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
        
        if (_characterController.isGrounded)
        {
            _doubleJumped = false;
            _canWallJump = false;
        }
    }

    private void CalculateHorizontalInput(Vector2 move)
    {
        _horizontalInput = move.x;
    }

    private void CalculateVelocity()
    {
        // x velocity
        if (_useJumpVelocityLeft)
            _horizontalMove = _wallJumpPush;
        else if (_useJumpVelocityRight)
            _horizontalMove = _wallJumpPush * -1;
        else
            _horizontalMove = _horizontalInput;
        
        var direction = new Vector3(_horizontalMove, 0, 0);
        _velocity = direction * _speed;

        // y velocity - gravity
        if (!_characterController.isGrounded)
            _yVelocity -= _gravity * Time.deltaTime;
        
        _velocity.y = _yVelocity;
    }

    private void Jump(InputAction.CallbackContext objContext)
    {
        if (_canWallJump)
        {
            if (Mathf.Approximately(_angle, 0)) //left
            {
                _useJumpVelocityLeft = true;
                WallJump();
            }

            if (Mathf.Approximately(_angle, 180)) // right
            {
                _useJumpVelocityRight = true;
                WallJump();
            }
        }
        else
        {
            if (_characterController.isGrounded)
                _yVelocity = _jumpHeight;
            else
            {
                if (_doubleJumped) return;
                _doubleJumped = true;
                _yVelocity = _jumpHeight;
            }
        }
        
        _canWallJump = false;
    }

    private void WallJump()
    {
        _doubleJumped = true;
        _yVelocity = _jumpHeight;
        StartCoroutine(WallJumpCoroutine(_wallJumpControlDelay));
    }
    
    private void FixedUpdate()
    {
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Movable"))
        {
            if (!hit.gameObject.GetComponent<Rigidbody>()) return; // null check
            var box = hit.collider.GetComponent<Rigidbody>();
            var pushDirection = new Vector3(hit.moveDirection.x, 0, 0);

            if (box is not null) 
                box.velocity = pushDirection * _boxPushPower;
        }
        
        // wall jump collision detection
        _angle = Vector3.Angle(hit.normal, Vector3.right);
        
        if (!_characterController.isGrounded)
        {
            if (Mathf.Approximately(_angle, 0)) //left
                _canWallJump = true;

            if (Mathf.Approximately(_angle, 180)) // right
                _canWallJump = true;
        }
    }

    private IEnumerator WallJumpCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _useJumpVelocityLeft = false;
        _useJumpVelocityRight = false;
    }
    
    private void OrbsCollected(int amountCollected)
    {
        orbs += amountCollected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
            OnDeath();
    }

    private void OnDeath()
    {
        lives--;
        UIManager.instance.UpdateLives();
        
        if (lives == 0)
        {
            _dead = true;
            _meshRenderer.enabled = false;
            UIManager.instance.GameOver();
        }
        else
            Respawn();
    }

    private void Respawn()
    {
        transform.position = _startPoint.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
    }
}
