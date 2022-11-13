using System;
using DG.Tweening;
using UnityEngine;

namespace World
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private ItemType itemType;

        private void Start()
        {
            var scale = transform.localScale;
            var animationSeconds = 0.2f;
            transform.DOScale(scale, animationSeconds).From(0f).SetEase(Ease.OutBack);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerLogic _))
            {
                inventorySystem.CollectItem(itemType);
                Destroy(gameObject);
            }
        }
    }
}