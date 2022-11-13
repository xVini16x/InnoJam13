using Events;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using World;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private ItemType _costsItem;
    [SerializeField] private int costAmount;
    [SerializeField] private InventorySystem _inventorySystem;
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject BiggerChicken;

    private void Update()
    {
        if (_inventorySystem.CouldUseItem(_costsItem, costAmount))
        {
            blocker.SetActive(false);
        }
        else
        {
            blocker.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isValidTrigger = false;
        if (other.TryGetComponent<AllyLogic>(out var allylogic))
        {
            isValidTrigger = true;
        }

        EnemyLogic enemyLogic = null;
        if (_upgradeType != UpgradeType.UpgradeChicken)
        {
            if (other.TryGetComponent<EnemyLogic>(out enemyLogic))
            {
                isValidTrigger = true;
            }

            if (other.TryGetComponent<NavMeshAgent>(out var agent))
            {
                if (!agent.enabled && other.transform.parent != null)
                {
                    isValidTrigger = false;
                }
            }

            if (other.CompareTag("Player"))
            {
                isValidTrigger = true;
            }
        }

        if (!isValidTrigger)
        {
            return;
        }

        if (!_inventorySystem.CouldUseItem(_costsItem, costAmount))
        {
            MessageBroker.Default.Publish(new SpawnParticle
            {
                Position = transform.position,
                Type = ParticleType.UpgradeFailed
            });
            return;
        }

        if (enemyLogic != null)
        {
            enemyLogic.Die();
        }

        if (allylogic != null)
        {
            if (_upgradeType == UpgradeType.UpgradeChicken)
            {
                rigidVelocity = allylogic.Rigid.velocity;
                position = allylogic.transform.position;
                rotation = allylogic.transform.rotation;
            }

            allylogic.Die();
        }

        Upgrade();
    }

    private Vector3 rigidVelocity;
    private Vector3 position;
    private Quaternion rotation;

    private void Upgrade()
    {
        switch (_upgradeType)
        {
            case UpgradeType.ChickenCapacity:
                AllySpawnerLogic.maxAllyCount += 2;
                break;
            case UpgradeType.UpgradeChicken:
                var go = Instantiate(BiggerChicken, null);
                AllySpawnerLogic.allyCount++;
                if (go.TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    rigidbody.velocity = rigidVelocity/4f;
                }

                if (go.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                {
                    navMeshAgent.enabled = false;
                }

                go.transform.position = position;
                go.transform.rotation = rotation;
                break;
        }

        _inventorySystem.TryUseItem(_costsItem, costAmount);

        MessageBroker.Default.Publish(new SpawnParticle
        {
            Position = transform.position,
            Type = ParticleType.Upgrade
        });
    }
}

public enum UpgradeType
{
    ChickenCapacity,
    UpgradeChicken
}
