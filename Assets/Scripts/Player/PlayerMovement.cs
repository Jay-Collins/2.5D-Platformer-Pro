using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

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

    private float _yVelocity;
    private bool _doubleJumped;
    private bool _dead;

    private void OnEnable()
    {
        lives = 3;
        
        //subscriptions
        Collectables.orbCollected += OrbsCollected;

        //reference null checks
        if (_characterController is null)
            Debug.Log("CharacterController is NULL");
    }

    private void Update()
    {
        CalculateVelocity();
    }

    private void CalculateVelocity()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var direction = new Vector3(horizontalInput, 0, 0);
        _velocity = direction * _speed;

        if (_characterController.isGrounded)
        {
            _doubleJumped = false;
            
            if (Input.GetKeyDown(KeyCode.Space))
                _yVelocity = _jumpHeight;
        }
        else
        {
            if (!_doubleJumped && Input.GetKeyDown(KeyCode.Space))
            {
                _doubleJumped = true;
                _yVelocity = _jumpHeight;
            }
                
            _yVelocity -= _gravity * Time.deltaTime;
        }

        _velocity.y = _yVelocity;
        
        if (_dead && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
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
