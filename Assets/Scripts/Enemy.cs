using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private bool _isMoving;

    public event Action<Enemy> Death;

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetStartRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
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
    }

    private IEnumerator MoveLoop()
    {
        while (_isMoving)
        {
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
