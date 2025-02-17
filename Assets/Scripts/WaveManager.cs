using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private int _enemiesToAddEachWave = 3;
    
    [SerializeField]
    private int _startingWaveNumber = 1;
    
    private int _enemiesKilled;
    private int _spawnedEnemies;
    private int _maxEnemiesToSpawn;
    
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    
    void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("No spawn manager found");
        
        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("No UI manager found");
        
    }
    
    public void StartFirstWave()
    {
        InitializeWave();
        _spawnManager.StartSpawn();
    }
    
    private void StartNextWave()
    {
        InitializeWave();
        _spawnManager.StartEnemySpawning();
    }

    private void InitializeWave()
    {
        _uiManager.ShowWave(_startingWaveNumber);
        _maxEnemiesToSpawn = _startingWaveNumber++ * _enemiesToAddEachWave;
        _enemiesKilled = 0;
        _spawnedEnemies = 0;
    }

    public void AddEnemyToWave()
    {
        if (++_spawnedEnemies == _maxEnemiesToSpawn) _spawnManager.StopEnemySpawning();
    }
    
    public void EnemyKilled()
    {
        _enemiesKilled++;
        if (_enemiesKilled == _maxEnemiesToSpawn)
            StartNextWave();
    }
}
