using Events;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.View
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private Slider playerHealth;

        private void Awake()
        {
            MessageBroker.Default.Receive<PlayerHealthChanged>()
                .TakeUntilDestroy(this)
                .Subscribe(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged(PlayerHealthChanged data)
        {
            playerHealth.value = data.NewPlayerHealth / data.MaxPlayerHealth;
        }
    }
}