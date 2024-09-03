using System.Collections;
using UnityEngine;

namespace RangerProject.Scripts.Gameplay
{
    [CreateAssetMenu(menuName = "Camera/NewCameraShake", fileName = "NewCameraShake")]
    public class CameraSettings : ScriptableObject
    {
        [Header("General")] [SerializeField] private float CameraFollowSpeed = 10.0f;
        
        [Header("CameraShakeSettings")]
        [SerializeField, Min(0)] private int TimesToShake = 1;
        [SerializeField, Min(0)] private float Intensity = 1.0f;
        [SerializeField, Min(0)] private float Snappiness = 10.0f;
        [SerializeField, Min(0)] private float ReturnSpeed = 10.0f;
        [SerializeField, Min(0)] private float ShakeDuration = 1.0f;
       
        public int GetTimesToShake() => TimesToShake;

        public float GetCameraFollowSpeed() => CameraFollowSpeed;
        public float GetIntensity() => Intensity;
        
        public float GetSnappiness() => Snappiness;
        public float GetCameraReturnSpeed() => ReturnSpeed;
        public float GetShakeDuration() => ShakeDuration;
    }
}
