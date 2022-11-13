using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySystem", menuName = "ScriptableObjects/InventorySystem", order = 1)]
public class InventorySystem : ScriptableObjectSystemBase
{
    #region Public Fields

    public List<Item> Items;

    #endregion

    #region Public methods

    public void CollectItem(ItemType itemType, int amount = 1)
    {
        Func<Item, bool> condition = item => item.ItemType == itemType;
        if (Items.Any(condition))
        {
            Items.FirstOrDefault(condition).Count += amount;
        }
        else
        {
            Items.Add(new Item(itemType, amount));
        }

        var currentAmount = Items.FirstOrDefault(condition)!.Count += amount;

        MessageBroker.Default.Publish(new ItemAmountChanged
        {
            ItemType = itemType,
            NewCurrency = currentAmount,
        });
    }

    public bool TryUseItem(ItemType itemType, int amount = 1)
    {
        Func<Item, bool> condition = item => item.ItemType == itemType;
        if (!Items.Any(condition))
        {
            Debug.LogError("We dont have that item currently");
            return false;
        }

        {
            var item = Items.FirstOrDefault(condition);
            if (item.Count < amount)
            {
                Debug.LogError("we dont have enough items");
                return false;
            }

            item.Count -= amount;
            return true;
        }
    }

    #endregion

    #region Private methods
    
    public override void Init()
    {
        Items = new List<Item>();
    }

    #endregion
}

[Serializable]
public class Item
{
    #region Public Fields

    public int Count;
    public ItemType ItemType;

    #endregion

    #region Constructors

    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }

    #endregion
}
