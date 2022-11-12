using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildCommand", menuName = "ScriptableObjects/Commands/BuildCommand", order = 1)]
public class BuildCommand : ICommand
{
	#region Serialize Fields

	[SerializeField] private BuildingSystem _buildingSystem;
	[SerializeField] private InventorySystem _inventorySystem;

	#endregion

	#region Private Fields

	[NonSerialized] private BuildCommandSettings _buildCommandSettings;

	#endregion

	#region Properties

	public BuildingSystem BuildingSystem => _buildingSystem;
	public InventorySystem InventorySystem => _inventorySystem;

	#endregion

	#region Public methods

	public void InitCommand(BuildCommandSettings commandSetttings)
	{
		_buildCommandSettings = commandSetttings;
	}

	#endregion

	#region ICommand Members

	public override bool DoCommand()
	{
		if (InventorySystem.TryUseItem(_buildCommandSettings.ItemTypeToBuild))
		{
			return BuildingSystem.TryToPlaceObject(_buildCommandSettings.PlacementPosition);
		}

		return false;
	}

	#endregion
}

public class BuildCommandSettings : CommandSetttings
{
	#region Public Fields

	public ItemType ItemTypeToBuild;
	public Vector3 PlacementPosition;

	#endregion
}
