using System;
using UnityEngine;

public class LifeArtifactLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    private float _health;

    private void Start()
    {
        _health = maxHealth;
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
        _health = health;
        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}