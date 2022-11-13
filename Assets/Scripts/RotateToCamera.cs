using System;
using TMPro;
using UnityEngine;

namespace StarterAssets
{
	public class RotateToCamera:MonoBehaviour
	{
		[SerializeField] private InventorySystem _inventorySystem;
		[SerializeField] private bool inventoryCheck = true;
		[SerializeField] private ItemType _itemType;
		[SerializeField] private int amount;
		[SerializeField] private TextMeshProUGUI text;
		
		private void Update()
		{
			transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward * -1);
			if (inventoryCheck)
			{
				if (_inventorySystem.CouldUseItem(_itemType, amount))
				{
					text.color = Color.green;
				}
				else
				{
					text.color = Color.red;
				}
			}
		}
	}
}
