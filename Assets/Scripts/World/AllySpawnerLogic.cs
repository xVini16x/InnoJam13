using System;
using UniRx;
using UnityEngine;

namespace World
{
    public class AllySpawnerLogic : MonoBehaviour
    {
        [SerializeField] private GameObject allyPrefab;
        [SerializeField] private float spawnIntervalSeconds;
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
            Instantiate(allyPrefab, spawnPosition.position, Quaternion.identity, null);
        }
    }
}