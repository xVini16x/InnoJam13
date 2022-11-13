using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerLogic player))
        {
            player.SetHealth(0);
        }
        
        if (other.TryGetComponent(out EnemyLogic e))
        {
            e.Die();
        }
        
        if (other.TryGetComponent(out AllyLogic a))
        {
            a.Die();
        }
    }
}
