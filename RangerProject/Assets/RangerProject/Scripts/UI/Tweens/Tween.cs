using System;
using System.Collections;
using UnityEngine;

namespace RangerProject.Scripts.UI.Tweens
{
    public abstract class Tween : MonoBehaviour
    {
        [SerializeField] protected bool PlayOnStart = false;
        [SerializeField] protected float PlayTime;
        
        private void OnEnable()
        {
            if (PlayOnStart)
            {
                StartCoroutine(PlayTween());
            }
        }

        protected abstract IEnumerator PlayTween();
    }
}
