using System;
using UnityEngine;
using UnityEngine.AI;
using World;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float damageAfterThrow = 5f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyView enemyView;
    [SerializeField] private float navMeshDistanceCheck = 0.5f;
    [SerializeField] private Rigidbody _rigidbody;
    private float lastCheckTime;
    private float _health;
    private bool isDead;

    private void Start()
    {
        _health = maxHealth;
    }

    private void Update()
    {
        var artifact = FindObjectOfType<LifeArtifactLogic>();
        if (artifact != null && agent.enabled)
        {
            agent.SetDestination(artifact.transform.position);
        }
        
        if (!agent.enabled && _rigidbody.velocity.magnitude < 0.3f && Time.timeSinceLevelLoad - lastCheckTime > 1f &&
            transform.parent == null)
        {
            lastCheckTime = Time.timeSinceLevelLoad;
            if (NavMesh.SamplePosition(transform.position, out var hit, navMeshDistanceCheck, -1))
            {
                DealDamage(damageAfterThrow);
                agent.enabled = true;
                _rigidbody.isKinematic = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!agent.enabled)
        {
            return;
        }
        if (!collision.transform.TryGetComponent<AllyLogic>(out var al))
        {
            return;
        }

        if (al.NavMeshAgent.enabled)
        {
            return;
        }
        Die();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.GetComponent<AllyLogic>() != null)
        {
            // TODO: let enemy logic configure their attack strength
            SetHealth(_health - 2f * Time.deltaTime);
        }
    }

    private void SetHealth(float health)
    {
        _health = health;
        if (_health <= 0f && !isDead)
        {

            Die();
        }
    }
    public void DealDamage(float damage)
    {
        SetHealth(_health - damage);
    }

    private void Die()
    {
        isDead = true;
        enemyView.Die();
        Destroy(gameObject, 2f);
    }
}
