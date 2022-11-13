using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class UpDownAnimation : MonoBehaviour
    {
        [SerializeField] private float distance = 2f;
        [SerializeField] private float animationTime = 2f;

        private void Start()
        {
            transform.DOLocalMoveY(distance, animationTime)
                .SetRelative(true)
                .SetDelay(Random.value)
                .SetEase(Ease.InOutCubic)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}