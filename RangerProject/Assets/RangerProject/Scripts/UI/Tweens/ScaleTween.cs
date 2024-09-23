using System.Collections;
using UnityEngine;

namespace RangerProject.Scripts.UI.Tweens
{
    public class ScaleTween : Tween
    {
        [SerializeField] private Vector3 From;
        [SerializeField] private Vector3 To;


        protected override IEnumerator PlayTween()
        {
            float PassedTime = 0.0f;

            while (PassedTime < PlayTime)
            {
                PassedTime += Time.deltaTime;

                transform.localScale = Vector3.Lerp(From, To, PassedTime / PlayTime);
                yield return null;
                
            }
        }
    }
}
