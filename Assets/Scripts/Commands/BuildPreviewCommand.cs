using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildPreviewCommand", menuName = "ScriptableObjects/Commands/BuildPreviewCommand", order = 1)]
public class BuildPreviewCommand : ICommand
{
	[SerializeField] private BuildingSystem _buildingSystem;
	[SerializeField] private InventorySystem _inventorySystem;
	[SerializeField] private BuildCommandSettings _buildCommandSettings;

	public BuildingSystem BuildingSystem => _buildingSystem;
	public InventorySystem InventorySystem => _inventorySystem;
	private RaycastHit[] _raycastHits = new RaycastHit[5];
	[SerializeField] private float rotationSpeed = 10f;
	public  static Quaternion LastRotation = Quaternion.identity;
	public override bool DoCommand(CommandExecuter executer)
	{
		if (executer.GetExecuterType() == ExecuterType.Player)
		{
			_buildCommandSettings.Placement  = Camera.main.transform;
			var mouseWheel = Input.mouseScrollDelta.y;
			if (mouseWheel != 0)
			{
				LastRotation *= Quaternion.Euler(new Vector3(0, 1, 0) * mouseWheel  * rotationSpeed);	
			}
		}

		bool canBuild = true;

		if (!InventorySystem.CouldUseItem(_buildCommandSettings.RequiresItemType, _buildCommandSettings.RequiredAmount))
		{
			canBuild = false;
		}

		if (Physics.RaycastNonAlloc(_buildCommandSettings.Placement.position , _buildCommandSettings.Placement.forward, _raycastHits)<=0)
		{
			canBuild = false;
		}

		Vector3 placementPosition = executer.GetExecuterTransform().position;
		if (executer.GetExecuterType() == ExecuterType.Player)
		{
			placementPosition = FindObjectOfType<PlayerLogic>().PickUpHostAnchor.position;
		}
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

			canBuild = true;

			placementPosition =  currentHit.point;
			break;
		}
		
		BuildingSystem.TrySpwanPreview(canBuild, _buildCommandSettings.ItemTypeToBuild, placementPosition, LastRotation);

		return true; // can always do a preview
	}
}
