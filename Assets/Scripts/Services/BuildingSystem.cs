using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSystem", menuName = "ScriptableObjects/BuildingSystem", order = 1)]
public class BuildingSystem : ScriptableObjectSystemBase
{
	#region Private Fields

	private Transform _anchor;
	private ReplacementHandler equippedObject;

	#endregion

	#region Public methods

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

	public bool TryGetReplaceableObject(Transform pickUpTransform, Transform pickUpAnchor, out ReplacementHandler replacementHandler, float PickUpRadius = 3f)
	{
		replacementHandler = null;
		var colliders = Physics.OverlapSphere(pickUpTransform.position, PickUpRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);
		foreach (var col in colliders)
		{
			if (col.TryGetComponent<ReplacementHandler>(out var newReplacementHandler))
			{
				equippedObject = newReplacementHandler.PickUpIfPossible(pickUpTransform, pickUpAnchor.position);
				replacementHandler = equippedObject;
				return true; //Only want to pickup first object -> maybe improve ordering when two objects are near?
			}
		}

		return false;
	}

	#endregion
}
