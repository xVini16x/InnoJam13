using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemType : ScriptableObject
{
	public GameObject PrefabForBuilding;
	public GameObject PreviewPrefabForBuildingSuccess;
	public GameObject PreviewPrefabForBuildingFail;
	public float SpawnYPosition = -0.3f;
	public Vector3 ExtentSizeForSpawning;
}
