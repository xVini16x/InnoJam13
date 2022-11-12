using Events;
using UniRx;
using UnityEngine;

public class AllyView : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int Walk = Animator.StringToHash("Walk");

    private void Start()
    {
        animator.SetBool(Walk, true);
        animator.keepAnimatorControllerStateOnDisable = true;
    }

    public void Die()
    {
        MessageBroker.Default.Publish(new SpawnParticle
        {
            Position = transform.position,
            Type = ParticleType.FeatherExplosion
        });
    }
}