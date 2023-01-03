using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    
    [Header("References")] 
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    public static int orbs;
    public static int lives;
    
    private Transform _parentObject;
    private Vector3 _velocity;

    private float _horizontalInput;
    private float _yVelocity;
    private bool _doubleJumped;
    private bool _dead;

    private void OnEnable()
    {
        lives = 3;
        
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
    }

    private void CalculateHorizontalInput(Vector2 move)
    {
        _horizontalInput = move.x;
    }

    private void CalculateVelocity()
    {
        var direction = new Vector3(_horizontalInput, 0, 0);
        _velocity = direction * _speed;
        
        if (!_characterController.isGrounded)
            _yVelocity -= _gravity * Time.deltaTime;
        
        _velocity.y = _yVelocity;
    }

    private void Jump(InputAction.CallbackContext objContext)
    {
        if (_characterController.isGrounded)
        {
            _doubleJumped = false;
            _yVelocity = _jumpHeight;
        }
        else
        {
            if (_doubleJumped) return;
            
            _doubleJumped = true;
            _yVelocity = _jumpHeight;
        }
    }
    
    private void FixedUpdate()
    {
        _characterController.Move(_velocity * Time.deltaTime);
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
}
