using System;
using RangerProject.Scripts.Enviroment;
using UnityEngine;

namespace RangerProject.Scripts.Player
{
    public class RoomConstrainer : MonoBehaviour
    {
        [SerializeField] private Room CurrentRoomToConstrainTo;

        private void Update()
        {
            if(CurrentRoomToConstrainTo.IsInsideRoom(transform.position))
                return;

            transform.position = CurrentRoomToConstrainTo.GetNearestPointInRoom(transform.position);
        }
    }
}
