using System;
using UnityEngine;

public class LoadingScreenBackground : MonoBehaviour
{
    public event Action OnAnimationEnded;

    public void AnimationEnds()
    {
        OnAnimationEnded?.Invoke();
    }
}
