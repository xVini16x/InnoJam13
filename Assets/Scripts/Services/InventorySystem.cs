using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : ScriptableObjectSystemBase
{
	public List<Item> Items;

	void Init()
	{
		Items = new List<Item>();
	}
	
	public void CollectItem(ItemType itemType, int Amount=1)
	{
		Func<Item,bool> condition = item => item.ItemType == itemType; 
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
		Func<Item,bool> condition = item => item.ItemType == itemType; 
		if (!Items.Any(condition))
		{
			Debug.LogError("We dont have that item currently");
			return false;
		}
		else
		{
			var item=Items.FirstOrDefault(condition);
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
}

[Serializable]
public class Item
{
	public ItemType ItemType;
	public int Count;

	public Item(ItemType itemType, int count)
	{
		ItemType = itemType;
		Count = count;
	}
}


