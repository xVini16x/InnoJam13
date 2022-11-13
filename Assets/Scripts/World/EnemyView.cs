using Events;
using UniRx;
using UnityEngine;

namespace World
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimation;

        private static readonly int Die1 = Animator.StringToHash("Die");

        public void OnDamage()
        {
            MessageBroker.Default.Publish(new SpawnParticle
                {
                    Position = transform.position,
                    Type = ParticleType.FightAction
                });
        }

        public void Die()
        {
            characterAnimation.SetTrigger(Die1);

            MessageBroker.Default.Publish(new SpawnParticle
            {
                Position = transform.position,
                Type = ParticleType.CharacterDeath
            });
        }
    }
}