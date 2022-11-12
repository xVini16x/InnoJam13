using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
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
        var artifact = FindObjectOfType<LifeArtifactLogic>();
        if (artifact != null)
        {
            agent.SetDestination(artifact.transform.position);
        }
    }
}
