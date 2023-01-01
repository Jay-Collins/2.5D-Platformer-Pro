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
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;

    [Header("References")] [SerializeField]
    private CharacterController _characterController;

    private Transform _parentObject;
    private Vector3 _velocity;

    private float _yVelocity;
    private bool _doubleJumped;
    public static int orbs;

    private void OnEnable()
    {
        //subscriptions
        Collectables.orbCollected += OrbsCollected;

        //reference null checks
        if (_characterController is null)
            Debug.Log("CharacterController is NULL");
    }

    private void Update()
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
    }

    private void FixedUpdate()
    {
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void OrbsCollected(int amountCollected)
    {
        orbs += amountCollected;
    }
}
