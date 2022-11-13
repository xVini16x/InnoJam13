using UnityEngine;

namespace World
{
    public class DamageSettings : MonoBehaviour
    {
        [SerializeField] private float attackPower = 4f;
        public float AttackPower => attackPower;
    }
}