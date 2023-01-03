using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    [Header("ID Must Match Panel")]
    [SerializeField] private int _elevatorID;

    [SerializeField] private Transform[] _waypoints;

    [SerializeField] private float _elevatorSpeed;
    [SerializeField] private float _pauseLength;

    private bool _canMove;
    private bool _direction; 
    private int _index;

    private void OnEnable()
    {
        gameObject.transform.position = _waypoints[0].position;
        ElevatorPannel.callElevator += CallElevator;
    }

    private void Update()
    {
        if (_canMove)
            DirectionCheck();
    }

    private void CallElevator(int ID)
    {
        if (ID == _elevatorID)
            _canMove = true;
    }

    private void DirectionCheck()
    {
        if (_index >= _waypoints.Length - 1)
        {
            _direction = !_direction;
            _index--;
        }
        else if (_index <= 0)
        {
            _direction = !_direction;
            _index++;
        }
        else
        {
            if (_direction)
                _index--;
            else
                _index++;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        for (var i = 0; i < _waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }
    }
}
