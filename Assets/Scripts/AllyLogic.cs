using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AllyLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private NavMeshAgent agent;

    private float _health;

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
        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
