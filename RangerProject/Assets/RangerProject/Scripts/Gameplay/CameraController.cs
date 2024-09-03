using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace RangerProject.Scripts.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform TargetTransform;
        [SerializeField] private CameraSettings CurrentSettings;
        [SerializeField, Min(0)] private float BackwardsOffset;
        [SerializeField, Min(0)] private float HeightOffset;
        
        private Vector3 TargetPosition = Vector3.zero;
        private Vector3 DefaultPosition = Vector3.zero;

        private bool bIsPlayingShake = false;

        private void Start()
        {
            CalculateCameraTransform();
            
            TargetPosition = DefaultPosition;
        }

        public void Update()
        {
            CalculateCameraTransform();
            
            TargetPosition = Vector3.Lerp(TargetPosition, DefaultPosition, Time.deltaTime * (bIsPlayingShake ? CurrentSettings.GetCameraReturnSpeed() : CurrentSettings.GetCameraFollowSpeed()));
            transform.position = Vector3.Slerp(transform.position, TargetPosition, Time.deltaTime * CurrentSettings.GetSnappiness());
        }
        
        public void PlayCameraShakeDirectional(CameraSettings settings, Vector3 Direction)
        {
            StartCoroutine(PlayDirectionalCameraShake(Direction, transform, settings));
        }

        void CalculateCameraTransform()
        {
            Vector3 PlayerPosition = TargetTransform.position;
            Vector3 Position = PlayerPosition + new Vector3(0, HeightOffset, -BackwardsOffset);

            float TargetAngle =  Mathf.Rad2Deg * Mathf.Atan(HeightOffset / BackwardsOffset);
            transform.rotation = Quaternion.Euler(new Vector3(90 - TargetAngle, 0, 0));
            
            DefaultPosition = Position;
        }
        
        IEnumerator PlayDirectionalCameraShake(Vector3 Direction, Transform CameraTransform, CameraSettings cameraSettings)
        {
            bIsPlayingShake = true;
            
            float DelayBetweenShakes = cameraSettings.GetShakeDuration() / cameraSettings.GetTimesToShake();
            WaitForSeconds WaitForSeconds = new WaitForSeconds(DelayBetweenShakes);
            
            for (int TimesAlreadyShaken = 0; TimesAlreadyShaken < cameraSettings.GetTimesToShake(); TimesAlreadyShaken++)
            {
                TargetPosition = CameraTransform.position + Direction * cameraSettings.GetIntensity();
                yield return WaitForSeconds;
            }

            bIsPlayingShake = false;
        }

    }
}
