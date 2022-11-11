using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))] //triggerCOllider for pickup logic
public class ReplacementHandler : MonoBehaviour
{
	public ItemType ItemType;
	public GameObject unplaced;
	public GameObject placed;

	private void Start()
	{
		SetVisualsToPlaced();
	}

	private void SetVisualsToPlaced()
	{
		placed.SetActive(true);
		unplaced.SetActive(false);
	}

	private void SetVisualsToUnplaced()
	{
		placed.SetActive(false);
		unplaced.SetActive(true);
	}

	public ReplacementHandler PickUpIfPossible(Transform playerTransform, Vector3 anchor)
	{
		transform.SetParent(playerTransform, true);
		transform.position = anchor;
		SetVisualsToUnplaced();
		return this;
	}

	public bool CanPlace(Vector3 targetPosition)
	{
		transform.parent = null;
		transform.position = targetPosition;
		SetVisualsToPlaced();
		return true;
	}
}
