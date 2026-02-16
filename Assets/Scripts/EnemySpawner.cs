using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private Target _target;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _enemyMaximalCount;
    [SerializeField] private int _enemyCapacity;

    private ObjectPool<Enemy> _poolOfEnemies;
    private bool isSpawning;

    public void SpawnEnemy()
    {
        _poolOfEnemies.Get();
    }
    
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

    private void OnGetFromPool(Enemy enemy)
    {
        enemy.SetStartPosition(GetStartPointPosition());
        enemy.SetTarget(_target);
        enemy.gameObject.SetActive(true);

        enemy.Death += DisactivateEnemy;
        enemy.TargetCatched += DisactivateEnemy;
    }

    private void OnReleaseToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        enemy.Death -= DisactivateEnemy;
        enemy.TargetCatched -= DisactivateEnemy;
    }
    
    private Vector3 GetStartPointPosition()
    {
        return _spawnPoint.GetPosition();
    }

    private void DisactivateEnemy(Enemy enemy)
    {
        _poolOfEnemies.Release(enemy);
    }
}