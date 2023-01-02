using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Action<Vector2> movement;
    public static Action<InputAction.CallbackContext> jumpStarted;
    public static Action<InputAction.CallbackContext> jumpCanceled;

    private PlayerInputActions _playerInput;

    private void OnEnable()
    {
        InitializeInputs();
        
        if (_playerInput == null)
            Debug.Log("_playerInput in InputManager is NULL");
    }

    private void InitializeInputs()
    {
        _playerInput = new PlayerInputActions();
        _playerInput.PlayerMovement.Enable();
        _playerInput.PlayerMovement.Jump.started += JumpStarted;
        _playerInput.PlayerMovement.Jump.canceled += JumpCanceled;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        var move = _playerInput.PlayerMovement.Movement.ReadValue<Vector2>();
        
        movement(move);
    }

    private void JumpStarted(InputAction.CallbackContext objContext)
    {
        if (_playerInput.PlayerMovement.enabled)
            jumpStarted(objContext);
    }
    
    private void JumpCanceled(InputAction.CallbackContext objContext)
    {
        if (_playerInput.PlayerMovement.enabled)
            jumpCanceled(objContext);
    }
}
