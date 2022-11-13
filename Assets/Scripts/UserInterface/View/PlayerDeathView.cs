using DG.Tweening;
using Events;
using UniRx;
using UnityEngine;

namespace UserInterface.View
{
    public class PlayerDeathView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start()
        {
            MessageBroker.Default.Receive<PlayerDeath>()
                .TakeUntilDestroy(this)
                .Subscribe(_ => ShortlyShowScreen());
        }

        private void ShortlyShowScreen()
        {
            canvasGroup.DOFade(1f, 1f).SetLoops(2, LoopType.Yoyo).From(0f);
        }
    }
}