using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    #region Singleton
    public static PlayerInstance Instance;
    private void Awake()
    {
        Instance = this;
        Player = transform;
    }
    #endregion

    public Transform Player { get; set; }
    public Transform PlayerHead;
    public Transform GroundPlayer;
}
