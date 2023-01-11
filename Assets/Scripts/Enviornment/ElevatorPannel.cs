using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorPannel : MonoBehaviour
{
    public static Action<int> callElevator;

    [Header("References")]
    [SerializeField] private MeshRenderer _elevatorLight;
    [SerializeField] private int _requiredOrbs;
    
    [Header("ID Must Match Platform")]
    [SerializeField] private int _elevatorID;

    private void OnEnable() => MovingPlatform.resetElevator += ElevatorReset;

    private void OnTriggerEnter(Collider other) => InputManager.interactStarted += Call;
    private void OnTriggerExit(Collider other) => InputManager.interactStarted -= Call;

    private void Call(InputAction.CallbackContext objContext)
    {
        if (PlayerMovement.orbs >= _requiredOrbs)
        {
            _elevatorLight.material.color = Color.green;
            callElevator?.Invoke(_elevatorID);
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void ElevatorReset(int ID)
    {
        if (ID == _elevatorID)
            _elevatorLight.material.color = Color.red;
    }
}
