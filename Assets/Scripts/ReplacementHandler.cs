using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))] //triggerCOllider for pickup logic
public class ReplacementHandler : MonoBehaviour
{
	public ItemType ItemType;
	public GameObject unplaced;
	public GameObject placed;
	private bool kinematicDefault = true;

	private void Start()
	{
		SetVisualsToPlaced();
		if (TryGetComponent<Rigidbody>(out var rigidbody))
		{
			kinematicDefault = rigidbody.isKinematic;
		}
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

	public ReplacementHandler PickUpIfPossible(Transform parent, Vector3 position)
	{
		transform.SetParent(parent, true);
		transform.position = position;
		if (transform.TryGetComponent<Rigidbody>(out var rigid))
		{
			rigid.isKinematic =true;
		}
		if (transform.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
		{
			navMeshAgent.enabled =false;
		}
		
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

	public bool CanThrow(Vector3 force)
	{
		if (!transform.TryGetComponent<Rigidbody>(out var rigid))
		{
			return false;
		}
		rigid.isKinematic =false;
		transform.parent = null;
		SetVisualsToPlaced();
		rigid.AddForce(force, ForceMode.Impulse);
		return true;
	}
}
