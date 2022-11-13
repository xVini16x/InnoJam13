using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class AllySpawnerLogic : MonoBehaviour
    {
        [SerializeField] private GameObject[] allyPrefabs;
        [SerializeField] private float spawnIntervalSeconds;
        public static  int maxAllyCount = 40;
        public static int allyCount = 0;
        [SerializeField] private Transform spawnPosition;

        private void Start()
        {
            maxAllyCount = 3;
            Observable.Interval(
                    TimeSpan.FromSeconds(spawnIntervalSeconds))
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => Spawn()
                );
        }

        private void Spawn()
        {
            if (allyCount >= maxAllyCount)
            {
                return;
            }

            allyCount++;
            var randomIndex = Random.Range(0, allyPrefabs.Length);
            Instantiate(allyPrefabs[randomIndex], spawnPosition.position, Quaternion.identity, null);
        }
    }
}
