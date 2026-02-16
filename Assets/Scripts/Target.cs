using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool _isMooving;
    private float _maximalMooving = 2f;

    private void OnEnable()
    {
        //_isMooving = true;
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
            //не работает движение
            transform.position = Vector3.MoveTowards(transform.right * _maximalMooving, transform.right * _maximalMooving * -1, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
