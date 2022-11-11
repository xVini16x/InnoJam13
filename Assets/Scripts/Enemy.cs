using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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
        var playerPosition = FindObjectOfType<LifeArtifact>().transform.position;
        agent.SetDestination(playerPosition);
    }
}
