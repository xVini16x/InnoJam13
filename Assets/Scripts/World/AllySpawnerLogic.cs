using System;
using UniRx;
using UnityEngine;
using UserInterface.View;
using Random = UnityEngine.Random;

namespace World
{
    public class AllySpawnerLogic : MonoBehaviour
    {
        [SerializeField] private GameObject[] allyPrefabs;
        [SerializeField] private float spawnIntervalSeconds;
        public static int maxAllyCount = 40;
        public static int allyCount = 0;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private float Health = 10f;

        private float _health;

        private void OnTriggerStay(Collider collisionInfo)
        {
            if (collisionInfo.transform.TryGetComponent(out EnemyLogic _))
            {
                SetHealth(_health - 2f * Time.deltaTime);
            }
        }

        private void SetHealth(float health)
        {
            if (health < _health)
            {
                transform.Shake();
            }

            _health = health;
            if (_health <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject, 2f);
        }

        private void Start()
        {
            _health = Health;
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