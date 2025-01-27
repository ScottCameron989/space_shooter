using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        public void StartSpawn()
        {
            if (_enemyRoutine == null)
               _enemyRoutine = StartCoroutine(SpawnEnemyRoutine());
            if (_powerUpRoutine == null)
             _powerUpRoutine = StartCoroutine(SpawnPowerUpRoutine());
        }
        
        IEnumerator SpawnEnemyRoutine()
        {
            yield return new WaitForSeconds(3f);
            while (!_stopSpawning) {
                GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9f,9f), 8.5f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;                
                yield return new WaitForSeconds(_enemySpawnRate);
            }
        }
        
        IEnumerator SpawnPowerUpRoutine()
        {
            yield return new WaitForSeconds(3f);
            while (!_stopSpawning)
            {
                GameObject powerUpPrefab = _powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Length)];
                Instantiate(powerUpPrefab, new Vector3(Random.Range(-9f, 9f), 8.5f, 0), Quaternion.identity);
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
