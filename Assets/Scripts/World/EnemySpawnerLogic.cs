using System;
using UniRx;
using UnityEngine;

public class EnemySpawnerLogic : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnIntervalSeconds;

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
        Instantiate(enemyPrefab, transform.position, Quaternion.identity, null);
    }
}
