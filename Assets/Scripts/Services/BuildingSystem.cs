using UnityEngine;
using World;

[CreateAssetMenu(fileName = "BuildingSystem", menuName = "ScriptableObjects/BuildingSystem", order = 1)]
public class BuildingSystem : ScriptableObjectSystemBase
{
	#region Private Fields

	private Transform _anchor;
	private ReplacementHandler equippedObject;

	#endregion

	#region Public methods

	public override void Init()
	{
		
	}

	public bool TryToPlaceObject(Transform target)
	{
		var targetPosition = target.position;
		if (equippedObject.CanPlace(targetPosition))
		{
			equippedObject = null;
			return true;
		}

		return false;
	}

	public bool TryToThrowObject(Vector3 force, out ReplacementHandler replacementHandler)
	{
		replacementHandler = equippedObject;
		
		if (equippedObject == null)
		{
//			Debug.LogError("no equiped item");
			return false;
		}
		
		if (equippedObject.CanThrow(force))
		{
			equippedObject = null;
			return true;
		}

		return false;
	}

	public bool TryGetReplaceableObject(Transform pickUpTransform, Transform pickUpAnchor, out ReplacementHandler replacementHandler, float PickUpRadius = 3f)
	{
		replacementHandler = null;
		if (equippedObject != null)
		{
			return false;
		}
		var colliders = Physics.OverlapSphere(pickUpTransform.position, PickUpRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);
		foreach (var col in colliders)
		{
			if (col.TryGetComponent<ReplacementHandler>(out var newReplacementHandler))
			{
				equippedObject = newReplacementHandler.PickUpIfPossible(pickUpAnchor, pickUpAnchor.position);
				replacementHandler = equippedObject;
				return true; //Only want to pickup first object -> maybe improve ordering when two objects are near?
			}
		}

		return false;
	}

	#endregion

	public bool TryToSpwanObject(ItemType prefabForBuilding, Vector3 placementPos, Quaternion rot)
	{
		placementPos.y = prefabForBuilding.SpawnYPosition;
		if (prefabForBuilding.PrefabForBuilding.TryGetComponent<Collider>(out var collider))
		{
			var col = Physics.OverlapBox(placementPos, prefabForBuilding.ExtentSizeForSpawning, rot);
			for (int i = 0; i < col.Length; i++)
			{
				var current = col[i];
				if (current.CompareTag("Player"))
				{
					continue;
				}
				if (current.isTrigger)
				{
					continue;
				}
				if (current.TryGetComponent<BuildableArea>(out var area))
				{
					continue;
				}
				else
				{
					Debug.Log("Cannot spawn here "+col[i].gameObject.name);
					return false;
				}
			}
		}
		UnityEngine.GameObject.Instantiate(prefabForBuilding.PrefabForBuilding, placementPos, rot);
		return true;
	}

	public bool TrySpwanPreview(bool canBuild, ItemType item, Vector3 placementPos, Quaternion rot)
	{
		placementPos.y = item.SpawnYPosition;
		if (item.PrefabForBuilding.TryGetComponent<Collider>(out var collider))
		{
			var col = Physics.OverlapBox(placementPos, item.ExtentSizeForSpawning, rot, -1, QueryTriggerInteraction.Collide);
			for (int i = 0; i < col.Length; i++)
			{
				var current = col[i];
				if (current.TryGetComponent<AllySpawnerLogic>(out var _))
				{
					canBuild = false;
					break;
				}
				if (current.CompareTag("Player"))
				{
					continue;
				}
				if (current.isTrigger)
				{
					continue;
				}
				if (current.TryGetComponent<BuildableArea>(out var area))
				{
					continue;
				}
				else
				{
					canBuild = false;
					break;
				}
			}
		}

		var objectToSpawn = canBuild ? item.PreviewPrefabForBuildingSuccess : item.PreviewPrefabForBuildingFail;
		
		if (currenPreviewObject == null ||(currenPreviewObject!=null && currenPreviewSuccess !=canBuild))
		{
			if (currenPreviewObject != null)
			{
				Destroy(currenPreviewObject);
			}
			currenPreviewObject = UnityEngine.GameObject.Instantiate(objectToSpawn, placementPos, Quaternion.identity);
			currenPreviewObject.transform.GetChild(0).localRotation = rot;
			currenPreviewSuccess = canBuild;
			return true;
		}
		
		currenPreviewObject.transform.position = placementPos;
		currenPreviewObject.transform.GetChild(0).localRotation = rot;
		return true;
	}

	private GameObject currenPreviewObject;
	private bool currenPreviewSuccess;

	public void KillPreview()
	{
		Destroy(currenPreviewObject);
	}
}
