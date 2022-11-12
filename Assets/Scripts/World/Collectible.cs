using UnityEngine;

namespace World
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private ItemType itemType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerLogic _))
            {
                inventorySystem.CollectItem(itemType);
            }
        }
    }
}