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
    public bool CanDealDamage => agent.enabled;

    private void Start()
    {
        _health = maxHealth;
        target = FindObjectOfType<LifeArtifactLogic>().gameObject;
        if (UnityEngine.Random.Range(0, 3) == 0)
        {
            var house=FindObjectOfType<AllySpawnerLogic>();

            if (house != null)
            {
                target = house.gameObject;
            }
             
        }
    }

    private GameObject target;
    private void Update()
    {
        
        if (target != null && agent.enabled)
        {
            agent.SetDestination(target.transform.position);
        }
        
        if (!agent.enabled && _rigidbody.velocity.magnitude < 0.3f && Time.timeSinceLevelLoad - lastCheckTime > 1f &&
            transform.parent == null)
        {
            lastCheckTime = Time.timeSinceLevelLoad;
            if (NavMesh.SamplePosition(transform.position, out _, navMeshDistanceCheck, -1))
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
        if (!agent.enabled)
        {
            return;
        }
        if (other.transform.GetComponent<AllyLogic>() != null)
        {
            // TODO: let enemy logic configure their attack strength
            DealDamage(2f * Time.deltaTime);
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

    private void DealDamage(float damage)
    {
        if (Random.value < 0.1f)
        {
            enemyView.OnDamage();
        }
        SetHealth(_health - damage);
    }

    public void Die()
    {
        agent.enabled = false;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity=Vector3.zero;
        WaveManager.EnemyDeaths++;
        isDead = true;
        enemyView.Die();
        Destroy(gameObject, 2f);
    }
}
