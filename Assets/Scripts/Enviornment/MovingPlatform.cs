using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform settings")]
    [SerializeField] private bool _loop;
    [SerializeField] private float _platformSpeed;
    
    [Header("Waypoints must be in order")]
    [SerializeField] private Transform[] _waypoints;
    
    private int _index = 1;
    private bool _direction;

    private void OnEnable()
    {
        gameObject.transform.position = _waypoints[0].position;
    }

    private void Update()
    {
        if (transform.position == _waypoints[_index].position)
            DirectionCheck();
    }
    

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position,_waypoints[_index].position,_platformSpeed * Time.deltaTime);
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
        
        for (var i = 0; i < _waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }

        if (_loop)
        {
            Gizmos.DrawLine(_waypoints[_waypoints.Length - 1].position, _waypoints[0].position);
        }
    }
}
