using Events;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.View
{
    public class LifeArtifactView : MonoBehaviour
    {
        [SerializeField] private Image health;

        private void Awake()
        {
            MessageBroker.Default.Receive<LifeArtifactHealthChanged>()
                .TakeUntilDestroy(this)
                .Subscribe(LifeArtifactHealthChanged);
        }

        private void LifeArtifactHealthChanged(LifeArtifactHealthChanged data)
        {
            health.fillAmount = data.NewArtifactHealth / data.MaxArtifactHealth;
        }
    }
}