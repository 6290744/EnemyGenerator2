using UnityEngine;
using System.Collections;

public class SpawnInvoker : MonoBehaviour
{
    [SerializeField] private EnemySpawner[]  _spawners;
    [SerializeField] private int _spawnLatency = 2;
    
    private bool _isSpawning;

    private void Start()
    {
        _isSpawning = true;
        
        StartCoroutine(Spawnloop());
    }

    private IEnumerator Spawnloop()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnLatency);
        
        while (_isSpawning)
        {
            yield return waitForSeconds;
            
            GetRandomSpawner().SpawnEnemy();
        }
    }

    private EnemySpawner GetRandomSpawner()
    {
        return _spawners[Random.Range(0, _spawners.Length)];
    }
}
