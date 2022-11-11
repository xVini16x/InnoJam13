using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSystem", menuName = "ScriptableObjects/BuildingSystem", order = 1)]
public class BuildingSystem : ScriptableObjectSystemBase
{
	#region Private Fields

	private Transform _anchor;
	private ReplacementHandler equippedObject;

	#endregion

	#region Public methods

	public bool TryToPlaceObject(Vector3 targetPosition)
	{
		if (equippedObject.CanPlace(targetPosition))
		{
			equippedObject = null;
			return true;
		}

		return false;
	}

	public bool TryGetReplaceableObject(Transform playerTransform, Vector3 pickUpAnchor, out ReplacementHandler replacementHandler, float PickUpRadius = 3f)
	{
		replacementHandler = null;
		var colliders = Physics.OverlapSphere(playerTransform.position, PickUpRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);
		foreach (var col in colliders)
		{
			if (col.TryGetComponent<ReplacementHandler>(out var newReplacementHandler))
			{
				equippedObject = newReplacementHandler.PickUpIfPossible(playerTransform, pickUpAnchor);
				replacementHandler = equippedObject;
				return true; //Only want to pickup first object -> maybe improve ordering when two objects are near?
			}
		}

		return false;
	}

	#endregion
}
