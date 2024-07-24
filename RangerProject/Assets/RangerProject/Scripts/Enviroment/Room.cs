using System;
using UnityEngine;

namespace RangerProject.Scripts.Enviroment
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Bounds RoomBounds;
        [SerializeField] private MeshFilter RoomGroundPlane;

        public Bounds GetRoomBounds() => RoomBounds;
        public bool IsInsideRoom(Vector3 Position) => RoomBounds.Contains(Position);
        public Vector3 GetNearestPointInRoom(Vector3 Position) => RoomBounds.ClosestPoint(Position);
       
        private void OnDrawGizmosSelected()
        {
            if (transform.hasChanged && RoomGroundPlane)
            {
                var sharedMesh = RoomGroundPlane.sharedMesh;
                
                RoomBounds.center = transform.position + new Vector3(0, RoomBounds.extents.y, 0);
                RoomBounds.extents = new Vector3(sharedMesh.bounds.extents.x, RoomBounds.extents.y, sharedMesh.bounds.extents.z);
                Debug.Log("Centered bounds to this objects position");
            }
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(RoomBounds.center, RoomBounds.size);
        }
    }
}

