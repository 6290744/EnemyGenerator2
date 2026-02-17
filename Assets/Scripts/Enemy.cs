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

    public void Attack(Target target)
    {
        if (target is null)
        {
            return;
        }

        _target = target;

        _isMoving = true;

        StartCoroutine(MoveLoop());
    }

    private void OnDisable()
    {
        _isMoving = false;
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
            RotateTo(_target);

            MoveTo( _target.GetPosition());
            
            yield return null;
        }
    }

    private void RotateTo(Target target)
    {
        transform.rotation = Quaternion.LookRotation(target.GetPosition() - transform.position);
    }

    private void MoveTo(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }
}