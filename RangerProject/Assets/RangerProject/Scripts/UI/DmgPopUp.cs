using System;
using RangerProject.Scripts.Gameplay;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RangerProject.Scripts.UI
{
    public class DmgPopUp : MonoBehaviour
    {
        [SerializeField] private TMP_Text DmgText;
        [SerializeField] private Rigidbody Rigidbody;
        [SerializeField] private Vector3 MaxForce;
        [SerializeField] private Vector3 MinForce;
        [SerializeField, Min(0.0f)] private float PopUpPlayTime = 0.2f;

        private void OnEnable()
        {
            float XForce = Random.Range(MinForce.x, MaxForce.x);
            float YForce = Random.Range(MinForce.y, MaxForce.y);
            float ZForce = Random.Range(MinForce.z, MaxForce.z);

            Vector3 ForceToApply = new Vector3(XForce, YForce, ZForce);
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.AddForce(ForceToApply, ForceMode.Impulse);
        }

        private void Start()
        {
            DmgText = GetComponent<TMP_Text>();
        }

        public float GetPopUpPlayTime() => PopUpPlayTime;
        public void SetDmgPopUpText(string DmgAmount, Color Color)
        {
            DmgText.SetText(DmgAmount);
            DmgText.color = Color;
        }
    }
}
