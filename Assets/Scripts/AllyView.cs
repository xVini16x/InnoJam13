using UnityEngine;

public class AllyView : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int Walk = Animator.StringToHash("Walk");

    private void Start()
    {
        animator.SetBool(Walk, true);
        animator.keepAnimatorControllerStateOnDisable = true;
    }
}