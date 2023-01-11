using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public static Action<int> resetElevator;

    [Header("Platform settings")]
    [SerializeField] private bool _loop;
    [SerializeField] private bool _canMove;
    [SerializeField] private float _platformSpeed;
    [SerializeField] private float _pauseLength;

    [Header("Elevator Settings - ID Must Match Panel")] 
    [SerializeField] private bool _isElevator;
    [SerializeField] private bool _pressAgainToMove;
    [SerializeField] private bool _resetLight;
    [SerializeField] private int _elevatorID;
    
    [Header("Waypoints must be in order")]
    [SerializeField] private Transform[] _waypoints;
    
    private int _index = 1;
    private float _pauseTimer;
    private bool _direction;
    private bool _hasPauseTimer;

    private void OnEnable()
    {
        gameObject.transform.position = _waypoints[0].position;

        if (_isElevator)
        {
            ElevatorPannel.callElevator += CallElevator;
            _canMove = false;
        }
        
        _pauseTimer = _pauseLength;
        
        if (_pauseLength > 0)
            _hasPauseTimer = true;
    }

    private void Update()
    {
        MovementController();
    }
    
    private void FixedUpdate()
    {
        if (_canMove)
            transform.position = Vector3.MoveTowards(transform.position,_waypoints[_index].position,_platformSpeed * Time.deltaTime);
    }

    private void MovementController()
    {
        switch (_hasPauseTimer)
        {
            case false when transform.position == _waypoints[_index].position:
                if (_pressAgainToMove)
                {
                    _canMove = false;
                    
                    if (_resetLight)
                        resetElevator?.Invoke(_elevatorID);
                }
                DirectionCheck();
                break;
            case true when transform.position == _waypoints[_index].position:
            {
                if (_pauseTimer <= 0)
                {
                    _pauseTimer = _pauseLength;
                    if (!_pressAgainToMove)
                        _canMove = true;
                    DirectionCheck();
                }
                else
                {
                    _pauseTimer -= Time.deltaTime;
                    _canMove = false;
                }
                break;
            }
        }
    }

    private void DirectionCheck()
    {
        //if loop enabled
        if (_loop)
        {
            if (_index >= _waypoints.Length - 1)
            {
                _index = 0;
                return;
            }

            _index++;
            return;
        }
        
        //if loop not enabled
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
    
    private void CallElevator(int ID)
    {
        if (ID == _elevatorID)
            _canMove = !_canMove;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var scale = transform.localScale;
        
        for (var i = 0; i < _waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }
        
        foreach (var waypoint in _waypoints)
        {
            Gizmos.DrawWireCube(waypoint.position, new Vector3(scale.x,scale.y,scale.z));
        }

        if (_loop)
        {
            Gizmos.DrawLine(_waypoints[_waypoints.Length - 1].position, _waypoints[0].position);
        }
    }
}
