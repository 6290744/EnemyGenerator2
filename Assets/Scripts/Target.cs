using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject _centerOfRotation;
    [SerializeField] private float _rotationSpeed = 10;

    private bool _isMooving;

    private void OnEnable()
    {
        _isMooving = true;

        StartCoroutine(Moveloop());
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private IEnumerator Moveloop()
    {
        while (_isMooving)
        {
            transform.RotateAround(_centerOfRotation.transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }
}