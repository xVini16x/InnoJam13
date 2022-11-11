using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AllyLogic : MonoBehaviour
{
    [SerializeField] private uint maxHealth = 10;
    [SerializeField] private NavMeshAgent agent;

    private uint _health;

    private void Start()
    {
        _health = maxHealth;
    }

    private void Update()
    {
        var enemies = FindObjectsOfType<EnemyLogic>().Select(x => x.transform).ToArray();
        var closestEnemy = GetClosestEnemy(enemies);
        agent.SetDestination(closestEnemy.position);
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
}