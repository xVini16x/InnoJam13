using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AllyLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float navMeshDistanceCheck = 0.5f;
    [SerializeField] private AllyView view;

    private float lastCheckTime;
    private float _health;
    private bool isDead;

    private void Start()
    {
        _health = maxHealth;
    }

    private void FixedUpdate()
    {
        var enemies = FindObjectsOfType<EnemyLogic>().Select(x => x.transform).ToArray();
        if (enemies.Length > 0)
        {
            var closestEnemy = GetClosestEnemy(enemies);
            if (agent.enabled)
            {
                agent.SetDestination(closestEnemy.position);
            }
        }

        if (!agent.enabled && _rigidbody.velocity.magnitude < 0.3f && Time.timeSinceLevelLoad - lastCheckTime > 1f &&
            transform.parent == null)
        {
            lastCheckTime = Time.timeSinceLevelLoad;
            if (NavMesh.SamplePosition(transform.position, out var hit, navMeshDistanceCheck, -1))
            {
                agent.enabled = true;
            }
        }
    }

    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform closestTransform = null;
        float minimumDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float distance = Vector3.Distance(t.position, currentPos);
            if (distance < minimumDistance)
            {
                closestTransform = t;
                minimumDistance = distance;
            }
        }

        return closestTransform;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.GetComponent<EnemyLogic>() != null)
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
            Destroy(gameObject, 1f);
        }
    }

    private void Die()
    {
        isDead = true;
        view.Die();
    }
}