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
    public event Action<Enemy> TargetChased;

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

            TargetChased?.Invoke(this);
        }
    }
    
    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Chase(Target target)
    {
        if (target is null)
        {
            return;
        }

        _target = target;

        _isMoving = true;

        StartCoroutine(MoveLoop());
    }
    
    private IEnumerator MoveLoop()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        
        while (_isMoving)
        {
            RotateTo(_target);

            MoveTo(_target);
            
            yield return waitForEndOfFrame;
        }
    }

    private void RotateTo(Target target)
    {
        transform.rotation = Quaternion.LookRotation(target.Position - transform.position);
    }

    private void MoveTo(Target target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.Position, _moveSpeed * Time.deltaTime);
    }
}