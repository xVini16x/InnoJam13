using Events;
using UniRx;
using UnityEngine;
using UserInterface.View;

public class LifeArtifactLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    private float _health;

    private void Start()
    {
        SetHealth(maxHealth);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<EnemyLogic>() != null)
        {
            // TODO: let enemy logic configure their attack strength
            SetHealth(_health - 2f * Time.deltaTime);
        }
    }

    private void SetHealth(float health)
    {
        transform.Shake();
        _health = health;
        MessageBroker.Default.Publish(new LifeArtifactHealthChanged{
            NewArtifactHealth = _health,
            MaxArtifactHealth = maxHealth
        });
        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}