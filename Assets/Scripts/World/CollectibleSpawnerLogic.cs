using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace World
{
    public class CollectibleSpawnerLogic : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float spawnIntervalSeconds;

        private void Start()
        {
            Observable.Interval(
                    TimeSpan.FromSeconds(spawnIntervalSeconds))
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => TrySpawn()
                );
        }

        private void TrySpawn()
        {
            var spawnDistance = 4f;
            var randomPosition = transform.position + spawnDistance * Random.onUnitSphere;
            if (NavMesh.SamplePosition(randomPosition, out var hit, spawnDistance, NavMesh.AllAreas))
            {
                Instantiate(prefab, hit.position, Quaternion.identity, transform);
            }
        }
    }
}