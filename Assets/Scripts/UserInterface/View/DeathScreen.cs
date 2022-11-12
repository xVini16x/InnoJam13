using System;
using Events;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UserInterface.View
{
	public class DeathScreen:MonoBehaviour
	{
		[SerializeField] private Button TryAgainButton;
		[SerializeField] private CanvasGroup _canvasGroup;

		
		private void Awake()
		{
			TryAgainButton.onClick.AddListener(Retry);
			
		}
		
		private void Start()
		{
			MessageBroker.Default.Receive<PlayerHealthChanged>()
				.TakeUntilDestroy(this)
				.Subscribe(OnPlayerHealthChanged);
		}

		private void OnPlayerHealthChanged(PlayerHealthChanged data)
		{

			if (data.NewPlayerHealth <= 0)
			{
Activate();
			}
		}

		private void Retry()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

		public void Activate()
		{
			_canvasGroup.alpha = 1;
			_canvasGroup.interactable=true;
			_canvasGroup.blocksRaycasts=true;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
