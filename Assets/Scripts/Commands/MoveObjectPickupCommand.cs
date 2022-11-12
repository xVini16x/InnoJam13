

using System;
using UnityEngine;
using Object = System.Object;

[CreateAssetMenu(fileName = "MoveObjectPickupCommand", menuName = "ScriptableObjects/Commands/MoveObjectPickupCommand", order = 1)]
public class MoveObjectPickupCommand : ICommand
{
	[SerializeField] private InventorySystem _inventorySystem;
	[SerializeField] private BuildingSystem _buildingSystem;
	private MoveObjectPickupCommandSettings _moveObjectPickupCommandSettings;
	
	public override bool DoCommand(CommandExecuter executer)
	{
		if (executer.GetExecuterType() == ExecuterType.Player)
		{
			_moveObjectPickupCommandSettings = new PlayerMoveObjectPickupCommandSettings();
		}
		if (!_buildingSystem.TryGetReplaceableObject(_moveObjectPickupCommandSettings.HostTransform, _moveObjectPickupCommandSettings.Anchor, out var replacementHandler))
		{
			return false;
		}
		
		Debug.Log("Did collect "+replacementHandler.gameObject.name);
		_inventorySystem.CollectItem(replacementHandler.ItemType); 
		return true;
	}
}

public  class MoveObjectPickupCommandSettings:CommandSetttings
{
	public Transform HostTransform { get; set; }
	public Transform Anchor { get; set; }
}

public class PlayerMoveObjectPickupCommandSettings:MoveObjectPickupCommandSettings
{
	public PlayerMoveObjectPickupCommandSettings()
	{
		HostTransform=UnityEngine.Object.FindObjectOfType<PlayerLogic>().transform;
		Anchor=UnityEngine.Object.FindObjectOfType<PlayerLogic>().PickUpHostAnchor;	
	}
}
