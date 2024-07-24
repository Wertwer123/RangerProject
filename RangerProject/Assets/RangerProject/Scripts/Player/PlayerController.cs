using System;
using RangerProject.Scripts.Enviroment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RangerProject.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float MovementSpeed = 10.0f;
        [SerializeField] private PlayerInput PlayerInput;
        [SerializeField] private Rigidbody PlayerRB;

        private Vector3 WalkVector;
        
        private void Start()
        {
            PlayerInput.camera = Camera.current;
        }

        private void FixedUpdate()
        {
            PlayerRB.velocity = WalkVector;
            Debug.Log(PlayerRB.velocity);
        }

        private void Update()
        {
            
        }

        public void OnWalk(InputAction.CallbackContext CallbackContext)
        {
            Vector2 WalkInput = CallbackContext.ReadValue<Vector2>();
            WalkVector = new Vector3(WalkInput.x, 0, WalkInput.y) * MovementSpeed;
           
        }
        
        private void ConstrainPlayerToRoom()
        {
            
        }
    }
}

