using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using World;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private ItemType  _costsItem;
    [SerializeField] private int costAmount;
    [SerializeField] private InventorySystem _inventorySystem;
    private void OnTriggerEnter(Collider other)
    {
        bool isValidTrigger = false;
        if (other.TryGetComponent<AllyLogic>(out var allylogic))
        {
            isValidTrigger = true;
        }
        if (other.TryGetComponent<EnemyLogic>(out var enemyLogic))
        {
            isValidTrigger = true;
        }
        if (other.TryGetComponent<NavMeshAgent>(out var agent))
        {
            if (!agent.enabled && other.transform.parent!=null)
            {
                isValidTrigger = false;
            }
        }

        if (other.CompareTag("Player"))
        {
            isValidTrigger = true;
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
        Upgrade();
        if (enemyLogic != null)
        {
            enemyLogic.Die();
        }

        if (allylogic != null)
        {
            allylogic.Die();
        }
    }

    private void Upgrade()
    {
        switch (_upgradeType)
        {
           case UpgradeType.ChickenCapacity:
               AllySpawnerLogic.maxAllyCount += 2;
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
    ChickenCapacity
}
