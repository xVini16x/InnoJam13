using System;
using Events;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class AllyView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMesh;

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

    public void OnCollisionEnter(Collision collision)
    {
        if (navMesh.enabled || transform.parent!=null)
        {
            return;
        }
        if (collision.transform.TryGetComponent<BuildableArea>(out var area))
        {
            return;
        }

        if (collision.transform.TryGetComponent<AllyView>(out var _))
        {
            return;
        }

        if (collision.transform.CompareTag("Player"))
        {
            return;
        }
        MessageBroker.Default.Publish(new SpawnParticle
                                      {
                                          Position = transform.position,
                                          Type = ParticleType.FeatherExplosion
                                      });
    }
}
