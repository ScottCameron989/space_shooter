using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "ScriptableObject/WeightedSpawnDB", fileName = "SpawnDB")]
    public class WeightedSpawnDB : ScriptableObject
    {
        [Serializable]
        public class SpawnData
        {
            [SerializeField]
            public GameObject _prefabToSpawn;
            
            [Range(0f,100f)]
            [SerializeField]
            private float _spawnPrecent;
            
            [HideInInspector]
            public float _weight;
            
            public float GetSpawnChance() { return _spawnPrecent; }
        }
        
        [SerializeField]
        public SpawnData[] spawnData;
        
        public float _accumulatedWeights { get; private set;}
        
        
        public GameObject GetRandomSpawn()
        {
            float r = Random.Range(0f, 1f) * _accumulatedWeights;
            GameObject spawnPrefab = null;
            foreach(SpawnData spawn in spawnData)
            {
                if (spawn._weight >= r)
                {
                    spawnPrefab = spawn._prefabToSpawn;
                    break;
                }
            }
            if (spawnPrefab is null) Debug.LogError("Weight is out of range");
            return spawnPrefab;
        }
        
        public void OnValidate()
        {
            _accumulatedWeights = 0f;
            foreach(SpawnData spawnData in spawnData)
            {
                _accumulatedWeights += spawnData.GetSpawnChance();
                spawnData._weight = _accumulatedWeights;
            }
        }
        
    }