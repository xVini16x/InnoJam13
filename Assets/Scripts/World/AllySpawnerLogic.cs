using System;
using UniRx;
using UnityEngine;

namespace World
{
    public class AllySpawnerLogic : MonoBehaviour
    {
        [SerializeField] private GameObject allyPrefab;
        [SerializeField] private float spawnIntervalSeconds;
        public const  int maxAllyCount = 40;
        public static int allyCount = 0;
        [SerializeField] private Transform spawnPosition;

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
            if (allyCount >= maxAllyCount)
            {
                return;
            }

            allyCount++;
            Instantiate(allyPrefab, spawnPosition.position, Quaternion.identity, null);
        }
    }
}
