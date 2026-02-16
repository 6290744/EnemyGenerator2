using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _enemyMaximalCount;
    [SerializeField] private int _enemyCapacity;

    private ObjectPool<Enemy> _poolOfEnemies;
    private bool isSpawning;

    private void Awake()
    {
        _poolOfEnemies = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_enemyPrefab),
            actionOnGet: (enemy) => OnGetFromPool(enemy),
            actionOnRelease: (enemy) => OnReleaseToPool(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
            collectionCheck: true,
            defaultCapacity: _enemyCapacity,
            maxSize: _enemyMaximalCount
        );
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop(true));
    }

    private void OnGetFromPool(Enemy enemy)
    {
        enemy.SetStartPosition(GetStartPosition());
        enemy.SetStartRotation(GetStartRotation());
        enemy.gameObject.SetActive(true);

        enemy.Death += OnEnemyDeath;
    }

    private void OnReleaseToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        enemy.Death -= OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        _poolOfEnemies.Release(enemy);
    }

    private Vector3 GetStartPosition()
    {
        int choosedSpawnPointIndex = Random.Range(0, _spawnPoints.Count);

        return _spawnPoints[choosedSpawnPointIndex].GetSpawnPosition();
    }

    private Quaternion GetStartRotation()
    {
        int rotationX = 0;
        int rotationZ = 0;
        int rotationY = Random.Range(0, 360);
        
        return Quaternion.Euler(rotationX, rotationY, rotationZ);;
    }

    private IEnumerator SpawnLoop(bool isSpawning)
    {
         while (isSpawning)
         {
             if (_poolOfEnemies.CountActive < _enemyMaximalCount)
             {
                 _poolOfEnemies.Get();
             }
        
             yield return new WaitForSeconds(_spawnInterval);
         }
    }
}