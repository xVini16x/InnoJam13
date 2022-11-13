using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using World;

public class WaveManager:MonoBehaviour
	{
		[SerializeField]private WaveConfig _waveConfig;
		[SerializeField]private TextMeshProUGUI WaveText;
		[SerializeField]private TextMeshProUGUI WavePreloadText;
		[SerializeField]private TextMeshProUGUI WaveCountNumber;
		public static WaveSetting CurrentWave;
		private float StartTime;
		private int currentWaveIndex;
		private void Start()
		{
			currentWaveIndex = -1;
			NextWave();
		}

		private void Update()
		{
			if (EnemyDeaths >= CurrentWave.EnemyCount)
			{
				NextWave();
			}
		}

		private void NextWave()
		{
			WaveText.enabled = false;
			WavePreloadText.enabled = true;
			
			var Waves = _waveConfig.Waves;
			if (currentWaveIndex >= Waves.Count - 1)
			{
				Debug.Log("Last wave completed");
			}
			else
			{
				currentWaveIndex++;	
			}
			
			StartTime= Time.timeSinceLevelLoad;
			CanSpawn = false;
			EnemyDeaths = 0;
			CurrentWave = Waves[currentWaveIndex];
			EnemySpawnerLogic.maxSpawnAmount = CurrentWave.EnemyCount;
			EnemySpawnerLogic.spawnCount = 0;
			WaveCountNumber.text = CurrentWave.PrewarmTime.ToString();
			WaveCountNumber.transform.DOPunchScale(Vector3.one, 1f).OnComplete(() =>
			                                                                   {
				                                                                   UpdateWaveCounter();
			                                                                   });
		}

		void UpdateWaveCounter()
		{
			if (int.Parse(WaveCountNumber.text) <= 0)
			{
				WavePreloadText.enabled = false;
				WaveText.enabled = true;
				WaveCountNumber.text = (currentWaveIndex+1).ToString();
				CanSpawn = true;
			}
			else
			{
				WaveCountNumber.text = (int.Parse(WaveCountNumber.text) - 1).ToString();
				WaveCountNumber.transform.DOPunchScale(Vector3.one, 1f).OnComplete(() =>
				                                                                   {
					                                                                   UpdateWaveCounter();
				                                                                   });
			}
		}

		public static int EnemyDeaths;
		public static bool CanSpawn;
	}

[Serializable]
	public class WaveSetting
	{
		public float PrewarmTime;
		public int EnemyCount;
	}


