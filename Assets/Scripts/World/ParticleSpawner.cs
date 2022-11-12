using Events;
using UniRx;
using UnityEngine;

namespace World
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleConfigs[] particleConfigs;

        private void Start()
        {
            MessageBroker.Default.Receive<SpawnParticle>().TakeUntilDestroy(gameObject).Subscribe(ShowUi);
        }

        [System.Serializable]
        public struct ParticleConfigs
        {
            public ParticleType type;
            public GameObject particles;
        }

        private void ShowUi(SpawnParticle particles)
        {
            foreach (var particleConfig in particleConfigs)
            {
                if (particleConfig.type == particles.Type)
                {
                    Instantiate(particleConfig.particles, particles.Position, Quaternion.identity);
                    return;
                }
            }
        }
    }
}