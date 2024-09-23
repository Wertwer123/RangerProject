using System.Collections;
using System.Collections.Generic;
using RangerProject.Scripts.UI.Tweens;
using TMPro;
using UnityEngine;

public class TextAlphaTween : Tween 
{
    [SerializeField, Range(0,1)] private float From;
    [SerializeField, Range(0,1)] private float To;
    [SerializeField] private TMP_Text TargetText;
    
    protected override IEnumerator PlayTween()
    {
        float PassedTime = 0.0f;

        while (PassedTime < PlayTime)
        {
            PassedTime += Time.deltaTime;

            TargetText.alpha = Mathf.Lerp(From, To, PassedTime / PlayTime);
            yield return null;
        }
        Debug.Log("Finishedplaying tween");
    }
}
