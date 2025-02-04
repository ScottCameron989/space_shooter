using System;
using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObject/WeightedSpawnDB", fileName = "SpawnDB")]
    public class WeightedSpawnDB : ScriptableObject
    {
        [Serializable]
        internal class SpawnData
        {
            [SerializeField]
            private GameObject _prefabToSpawn;
            
            [Range(0f,100f)]
            [SerializeField]
            private float _spawnChance;
            
            [HideInInspector]
            public float _weight;
        }
        
        [SerializeField]
        private SpawnData[] spawnData;
        
    }