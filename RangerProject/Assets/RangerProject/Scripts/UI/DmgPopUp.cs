using System;
using TMPro;
using UnityEngine;

namespace RangerProject.Scripts.UI
{
    public class DmgPopUp : MonoBehaviour
    {
        [SerializeField] private TMP_Text DmgText;
        [SerializeField, Min(0.0f)] private float PopUpPlayTime = 0.2f;

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
