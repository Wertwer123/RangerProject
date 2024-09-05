using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound", fileName = "NewParameterizedAudioFile")]
public class ParamterizedAudiofile : ScriptableObject
{
   [SerializeField][Range(-3, 3)] float MinPitch;
   [SerializeField][Range(-3, 3)] float MaxPitch;
   [SerializeField][Range(0, 1)]  float Volume;
   [SerializeField]AudioClip AudioClip;
   

   public float GetMaxPitch() => MaxPitch;
   public float GetMinPitch() => MinPitch;
   public float GetVolume() => Volume;
   public AudioClip GetAudioClip() => AudioClip;
}
