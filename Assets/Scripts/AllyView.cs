using System;
using DG.Tweening;
using Events;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class AllyView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private Transform view;

    private static readonly int Walk = Animator.StringToHash("Walk");

    private void Start()
    {
        animator.SetBool(Walk, true);
        animator.keepAnimatorControllerStateOnDisable = true;

        var scale = transform.localScale;
        var animationSeconds = 0.2f;
        view.DOScale(scale, animationSeconds).From(0f).SetEase(Ease.OutBack);
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
        if (navMesh.enabled || transform.parent != null)
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