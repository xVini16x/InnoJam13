using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySystem", menuName = "ScriptableObjects/InventorySystem", order = 1)]
public class InventorySystem : ScriptableObjectSystemBase
{
	#region Public Fields

	public List<Item> Items;

	#endregion

	#region Public methods

	public void CollectItem(ItemType itemType, int Amount = 1)
	{
		Func<Item, bool> condition = item => item.ItemType == itemType;
		if (Items.Any(condition))
		{
			Items.FirstOrDefault(condition).Count += Amount;
		}
		else
		{
			Items.Add(new Item(itemType, Amount));
		}
	}

	public bool TryUseItem(ItemType itemType, int Amount = 1)
	{
		Func<Item, bool> condition = item => item.ItemType == itemType;
		if (!Items.Any(condition))
		{
			Debug.LogError("We dont have that item currently");
			return false;
		}

		{
			var item = Items.FirstOrDefault(condition);
			if (item.Count < Amount)
			{
				Debug.LogError("we dont have enough items");
				return false;
			}

			item.Count -= Amount;
			//item.Use();
			return true;
		}
	}

	#endregion

	#region Private methods

	void Init()
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