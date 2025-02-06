using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _enemySpawnRate = 5f;

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private WeightedSpawnDB _powerUpSpawnDB;

    [SerializeField]
    private Transform _enemyContainer;

    private bool _stopSpawning;

    private Coroutine _enemyRoutine;
    private Coroutine _powerUpRoutine;

    private Vector3 _nextSpawnPosition;

    public void Start()
    {
        if (_powerUpSpawnDB == null) Debug.LogError("No power up spawn DB Assigned");
    }

    public void StartSpawn()
    {
        _enemyRoutine ??= StartCoroutine(SpawnEnemyRoutine());
        _powerUpRoutine ??= StartCoroutine(SpawnPowerUpRoutine());
        _nextSpawnPosition.Set(0, 8.5f, 0);
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            _nextSpawnPosition.Set(Random.Range(-9f, 9f), _nextSpawnPosition.y, _nextSpawnPosition.z);
            GameObject newEnemy = Instantiate(_enemyPrefab, _nextSpawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            _nextSpawnPosition.Set(Random.Range(-9f, 9f), _nextSpawnPosition.y, _nextSpawnPosition.z);
            Instantiate(_powerUpSpawnDB.GetRandomSpawn(), _nextSpawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void StopSpawning(bool stop)
    {
        _stopSpawning = stop;
        if (_enemyRoutine != null) StopCoroutine(_enemyRoutine);
        if (_powerUpRoutine != null) StopCoroutine(_powerUpRoutine);
        _enemyRoutine = _powerUpRoutine = null;
    }
}