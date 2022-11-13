using System;
using UniRx;
using UnityEngine;

namespace World
{
    public class EnemySpawnerLogic : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnIntervalSeconds;
        public static int spawnCount=0;
        public static int maxSpawnAmount=0;
        private void Start()
        {
            Observable.Interval(
                    TimeSpan.FromSeconds(spawnIntervalSeconds))
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => Spawn()
                );
        }

        private void Spawn()
        {
            if (!WaveManager.CanSpawn)
            {
                return;
            }
            if (spawnCount >= maxSpawnAmount)
            {
                WaveManager.CanSpawn = false;
                return;
            }
            spawnCount++;
            Instantiate(enemyPrefab, transform.position, Quaternion.identity, null);
        }
    }
}
