using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private Target _target;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _enemyMaximalCount;
    [SerializeField] private int _enemyCapacity;

    private bool isSpawning;
    private ObjectPool<Enemy> _poolOfEnemies;

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
        enemy.SetStartPosition(GetSpawnPointPosition());
        enemy.gameObject.SetActive(true);
        enemy.Attack(_target);

        enemy.Death += DeactivateEnemy;
        enemy.TargetCatched += DeactivateEnemy;
    }

    private void OnReleaseToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        enemy.Death -= DeactivateEnemy;
        enemy.TargetCatched -= DeactivateEnemy;
    }
    
    private Vector3 GetSpawnPointPosition()
    {
        return _spawnPoint.GetPosition();
    }

    private void DeactivateEnemy(Enemy enemy)
    {
        _poolOfEnemies.Release(enemy);
    }
}