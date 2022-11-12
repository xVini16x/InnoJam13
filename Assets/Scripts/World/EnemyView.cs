using UnityEngine;

namespace World
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimation;

        private static readonly int Die1 = Animator.StringToHash("Die");

        public void Die()
        {
            characterAnimation.SetTrigger(Die1);
        }
    }
}