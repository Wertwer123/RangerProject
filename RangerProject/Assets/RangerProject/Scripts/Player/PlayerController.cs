using System;
using RangerProject.Scripts.Enviroment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RangerProject.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float MovementSpeed = 10.0f;
        [SerializeField] private float Gravity = -9.81f;
        [SerializeField] private float GravityMultiplier = 1.0f;
        [SerializeField] private float JumpForce = 10.0f; 
        [SerializeField] private PlayerInput PlayerInput;
        [SerializeField] private Rigidbody PlayerRB;
        [SerializeField] private string GroundTag = "Ground";

        private bool IsGrounded = false;
        private Vector3 Velocity;
        
        private void Start()
        {
            PlayerInput.camera = Camera.current;
        }

        private void FixedUpdate()
        {
            ApplyGravity();
            ApplyMovement();
        }
        
        public void OnJump(InputAction.CallbackContext CallbackContext)
        {
            if (CallbackContext.started)
            {
                Velocity.y += JumpForce;
                Debug.Log("Jumped");
            }
        }
        public void OnWalk(InputAction.CallbackContext CallbackContext)
        {
            Vector2 WalkInput = CallbackContext.ReadValue<Vector2>();
            Velocity = new Vector3(WalkInput.x * MovementSpeed, Velocity.y, WalkInput.y * MovementSpeed);
        }

        void ApplyGravity()
        {
            if (IsGrounded && Velocity.y < 0)
            {
                Velocity.y = -1.0f;
            }
            else
            {
                Velocity.y += Gravity * GravityMultiplier * Time.deltaTime;
            }
        }

        void ApplyMovement()
        {
            PlayerRB.velocity = Velocity;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(GroundTag))
            {
                IsGrounded = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag(GroundTag))
            {
                IsGrounded = false;
            }
        }
    }
}

