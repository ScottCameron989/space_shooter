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
        private GameObject[] _powerUpPrefabs;
        
        [SerializeField]
        private Transform _enemyContainer;
        
        private bool _stopSpawning = false;
        
        private Coroutine _enemyRoutine = null;
        private Coroutine _powerUpRoutine = null;
        
        private Vector3 _nextSpawnPosition;
        public void StartSpawn()
        {
            if (_enemyRoutine == null)
               _enemyRoutine = StartCoroutine(SpawnEnemyRoutine());
            if (_powerUpRoutine == null)
             _powerUpRoutine = StartCoroutine(SpawnPowerUpRoutine());
            
            _nextSpawnPosition.Set(0, 8.5f, 0 );
        }
        
        IEnumerator SpawnEnemyRoutine()
        {
            yield return new WaitForSeconds(3f);
            while (!_stopSpawning) {
                _nextSpawnPosition.Set(Random.Range(-9f,9f), _nextSpawnPosition.y,_nextSpawnPosition.z );
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
                _nextSpawnPosition.Set(Random.Range(-9f,9f), _nextSpawnPosition.y,_nextSpawnPosition.z );
                GameObject powerUpPrefab = _powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Length)];
                Instantiate(powerUpPrefab, _nextSpawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3, 8));
            }
        }
        public void StopSpawning( bool stop )
        {
            _stopSpawning = stop;
            if (_enemyRoutine != null) StopCoroutine(_enemyRoutine);
            if (_powerUpRoutine != null) StopCoroutine(_powerUpRoutine);
            _enemyRoutine = _powerUpRoutine = null;
        }
    }
