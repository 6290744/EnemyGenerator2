using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Target _target;
    private bool _isMoving;

    public event Action<Enemy> Death;
    public event Action<Enemy> TargetCatched;

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetTarget(Target target)
    {
        _target = target;
    }
    
    private void OnEnable()
    {
        _isMoving = true;

        StartCoroutine(MoveLoop());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out _))
        {
            _isMoving = false;

            Death?.Invoke(this);
        }
        else if (collision.gameObject.TryGetComponent(out Target target) && target == _target)
        {
            _isMoving = false;
            
            TargetCatched?.Invoke(this);
        }
    }

    private IEnumerator MoveLoop()
    {
        while (_isMoving)
        {
            Vector3 direction = _target.GetPosition();
            transform.Translate(direction * _moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
