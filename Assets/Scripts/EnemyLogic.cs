using UnityEngine;
using UnityEngine.AI;
using World;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyView enemyView;

    private float _health;
    private bool isDead;

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

    private void Die()
    {
        isDead = true;
        enemyView.Die();
        Destroy(gameObject, 2f);
    }
}
