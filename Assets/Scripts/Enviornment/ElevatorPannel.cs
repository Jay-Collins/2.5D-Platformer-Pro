using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPannel : MonoBehaviour
{
    [SerializeField] private MeshRenderer _elevatorLight;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("in zone");
            if (Input.GetKeyDown(KeyCode.E))
            {
               _elevatorLight.material.color = Color.green;
            }
        }
    }
}
