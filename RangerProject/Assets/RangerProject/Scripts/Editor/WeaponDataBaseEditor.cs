using System.Collections;
using System.Collections.Generic;
using RangerProject.Scripts.Player.WeaponSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponDataBase))]
public class WeaponDataBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("ReassignWeaponIDs"))
        {
            WeaponDataBase WeaponDataBase = (WeaponDataBase)target;
            WeaponDataBase.ReassignWeaponIDs();
        }
    }
}
