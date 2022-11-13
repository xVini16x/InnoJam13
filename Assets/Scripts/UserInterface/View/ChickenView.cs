
	using System;
	using TMPro;
	using UnityEngine;
	using World;

	public class ChickenView:MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI tmpField;

		private void Start()
		{
			AllySpawnerLogic.allyCount = 0;
		}

		private void Update()
		{
			var chickens = AllySpawnerLogic.allyCount;
			var maxChickens = AllySpawnerLogic.maxAllyCount;
			tmpField.text = chickens.ToString()+"/"+ maxChickens.ToString();
		}
	}
