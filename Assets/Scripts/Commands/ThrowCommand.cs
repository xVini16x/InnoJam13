using UnityEngine;



[CreateAssetMenu(fileName = "ThrowCommand", menuName = "ScriptableObjects/Commands/ThrowCommand", order = 1)]
public class ThrowCommand : ICommand
{
	[SerializeField] private BuildingSystem _buildingSystem;
	[SerializeField] private InventorySystem _inventorySystem;
	[SerializeField] private float throwForce = 10f;
	
	public override bool DoCommand(CommandExecuter executer)
	{
		var trans = executer.GetExecuterTransform();
		if (executer.GetExecuterType() == ExecuterType.Player)
		{
			trans = Camera.main.transform;
		}
		if (!_buildingSystem.TryToThrowObject(trans.forward*throwForce, out var replacementHandler))
		{
			return false;
		}
		
		_inventorySystem.TryUseItem(replacementHandler.ItemType); 
		return true;
	}
}
