using Events;
using TMPro;
using UniRx;
using UnityEngine;

namespace UserInterface.View
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI count;

        [SerializeField] private ItemType itemType;

        private void Awake()
        {
            MessageBroker.Default.Receive<ItemAmountChanged>()
                .TakeUntilDestroy(this)
                .Where(x => x.ItemType == itemType)
                .Subscribe(LifeArtifactHealthChanged);
        }

        private void LifeArtifactHealthChanged(ItemAmountChanged data)
        {
            count.transform.Shake();
            count.text = $"{data.NewCurrency} x";
        }
    }
}