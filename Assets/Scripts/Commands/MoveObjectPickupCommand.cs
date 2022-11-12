

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveObjectPickupCommand", menuName = "ScriptableObjects/Commands/MoveObjectPickupCommand", order = 1)]
public class MoveObjectPickupCommand : ICommand
{
	[SerializeField] private InventorySystem _inventorySystem;
	[SerializeField] private BuildingSystem _buildingSystem;
	[SerializeField] private MoveObjectPickupCommandSettings _moveObjectPickupCommandSettings;
	public override bool DoCommand(CommandExecuter executer)
	{
		if (!_buildingSystem.TryGetReplaceableObject(_moveObjectPickupCommandSettings.PickUpTransform, _moveObjectPickupCommandSettings.Anchor, out var replacementHandler))
		{
			return false;
		}
		
			//success
			_inventorySystem.CollectItem(replacementHandler.ItemType);
			return true;
	}
}

[Serializable]
public class MoveObjectPickupCommandSettings:CommandSetttings
{
	public Transform PickUpTransform;
	public Transform Anchor;
}
