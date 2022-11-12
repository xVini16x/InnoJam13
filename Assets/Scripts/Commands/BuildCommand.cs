using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildCommand", menuName = "ScriptableObjects/Commands/BuildCommand", order = 1)]
public class BuildCommand : ICommand
{
	[SerializeField] private BuildingSystem _buildingSystem;
	[SerializeField] private InventorySystem _inventorySystem;
	[SerializeField] private BuildCommandSettings _buildCommandSettings;

	public BuildingSystem BuildingSystem => _buildingSystem;
	public InventorySystem InventorySystem => _inventorySystem;
	private RaycastHit[] _raycastHits = new RaycastHit[5];
	public override bool DoCommand(CommandExecuter executer)
	{
		if (InventorySystem.TryUseItem(_buildCommandSettings.ItemTypeToBuild))
		{
			Transform mainTransform = Camera.main.transform;
			if (Physics.RaycastNonAlloc(mainTransform.position, mainTransform.forward, _raycastHits)>0)
			{
				for (int i = 0; i < _raycastHits.Length; i++)
				{
					var currentHit = _raycastHits[i];
					if (currentHit.transform == null)
					{
						continue;
					}
					if (!currentHit.transform.TryGetComponent<BuildableArea>(out var area))
					{
						continue;
					}

					_buildCommandSettings.PlacementPosition = currentHit.point;
					var placementPosition = _buildCommandSettings.PlacementPosition;
					return BuildingSystem.TryToPlaceObject(placementPosition);
				}
			}
		}

		return false;
	}
}

[Serializable]
public class BuildCommandSettings : CommandSetttings
{
	#region Public Fields

	public ItemType ItemTypeToBuild;
	[HideInInspector] // will set this by code for now
	public Vector3 PlacementPosition;

	#endregion
}
