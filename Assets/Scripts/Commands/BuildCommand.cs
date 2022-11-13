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
		_buildingSystem.KillPreview();
		var placementRotation = BuildPreviewCommand.LastRotation;
		if (executer.GetExecuterType() == ExecuterType.Player)
		{
			_buildCommandSettings.Placement  = Camera.main.transform;
			
		}
		
		if (InventorySystem.CouldUseItem(_buildCommandSettings.RequiresItemType, _buildCommandSettings.RequiredAmount))
		{
			
			if (Physics.RaycastNonAlloc(_buildCommandSettings.Placement.position , _buildCommandSettings.Placement.forward, _raycastHits)>0)
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

					var placementPosition =  currentHit.point;
					var didBuild= BuildingSystem.TryToSpwanObject(_buildCommandSettings.ItemTypeToBuild, placementPosition, placementRotation);
					if (didBuild)
					{
						InventorySystem.TryUseItem(_buildCommandSettings.RequiresItemType, _buildCommandSettings.RequiredAmount) ;
						return true;
					}
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
	public ItemType RequiresItemType;
	public int RequiredAmount;
	[HideInInspector] // will set this by code for now
	public Transform Placement;

	#endregion
}
