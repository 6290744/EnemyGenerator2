using UnityEngine;
using System.Collections;

public class MoverByWaypoints : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _minimalDistanceForChangeWaypoint = 0.05f;

    private bool _isMoving;
    private int _currentWaypointIndex;
    private Vector3 _currentWaypointPosition;
    
    private void OnEnable()
    {
        if (_wayPoints.Length > 0)
        {
            _currentWaypointIndex = GetStartWaypointIndex();

            _currentWaypointPosition = GetCurrentWaypointPosition();
        
            _isMoving = true;

            StartCoroutine(Moveloop());
        }
    }
    
    private IEnumerator Moveloop()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while (_isMoving)
        {
            Move();

            if (IsWaypointReached())
            {
                ChangeWaypoint();
            }

            yield return waitForEndOfFrame;
        }
    }
    
    private int GetStartWaypointIndex()
    {
        return Random.Range(0, _wayPoints.Length);
    }

    private int GetCurrentWaypointIndex()
    {
        return (_currentWaypointIndex + 1) % _wayPoints.Length;
    }

    private Vector3 GetCurrentWaypointPosition()
    {
        return _wayPoints[_currentWaypointIndex].position;
    }
    
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentWaypointPosition, _speed * Time.deltaTime);
    }

    private bool IsWaypointReached()
    {
        return Vector3.Distance(transform.position, _currentWaypointPosition) <= _minimalDistanceForChangeWaypoint;
    }

    private void ChangeWaypoint()
    {
        _currentWaypointIndex = GetCurrentWaypointIndex();

        _currentWaypointPosition = GetCurrentWaypointPosition();
    }
}
