using System;
using RangerProject.Scripts.Enviroment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RangerProject.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float MovementSpeed = 10.0f;
        [SerializeField] private float RotationSpeed = 1.0f;
        [SerializeField] private float MovementSpeedCrouching = 10.0f;
        [SerializeField, Range(0, 90)] private float MaxAngleBetweenLowerAndUpperBody = 20.0f;
        [SerializeField] private float Gravity = -9.81f;
        [SerializeField] private float GravityMultiplier = 1.0f;
        [SerializeField] private float JumpForce = 10.0f;
        [SerializeField, Range(0, 1.0f)] private float PlayerCrouchHeightMultiplier = 0.7f;
        [SerializeField] private string GroundTag = "Ground";
        [SerializeField] private CapsuleCollider PlayerCapsule;
        [SerializeField] private PlayerInput PlayerInput;
        [SerializeField] private Rigidbody PlayerRB;
        [SerializeField] private Transform UpperBody;
        [SerializeField] private Transform LowerBody;

        private float DefaultCapsuleHeight = 0.0f;
        private float AngleBetweenLowerAndUpperBoddy = 0.0f;
        private bool IsGrounded = false;
        private bool IsCrouching = false;
        private Vector3 Velocity = Vector3.zero;
        private Vector3 CurrentMousePositionWorld = Vector3.zero;
        private Vector3 CurrentLookDirection = Vector3.zero;
        
        private void Start()
        {
            PlayerInput.camera = Camera.main;
            DefaultCapsuleHeight = PlayerCapsule.height;
        }

        private void Update()
        {
            ApplyRotation();
        }

        private void FixedUpdate()
        {
            ApplyGravity();
            ApplyMovement();
        }

        public void Aim(InputAction.CallbackContext CallbackContext)
        {
            Transform Transform = transform;
            Vector3 Position = Transform.position;
            Vector2 MousePositionScreen = CallbackContext.ReadValue<Vector2>();
            
            Ray MousePositionRay = PlayerInput.camera.ScreenPointToRay(MousePositionScreen);
            Physics.Raycast(MousePositionRay, out RaycastHit Hit);
            CurrentMousePositionWorld = Hit.point;

            Vector3 DirectionToMouse = CurrentMousePositionWorld - Position;
            Vector3 LookDirection = Position - (Position + DirectionToMouse * 0.1f);
            
            LookDirection.y = 0;
            CurrentLookDirection = LookDirection;
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

            float XMovement = WalkInput.x;
            float ZMovement = WalkInput.y;
            
            Velocity = new Vector3(XMovement, Velocity.y, ZMovement);
        }

        public void OnCrouch(InputAction.CallbackContext CallbackContext)
        {
            if (CallbackContext.started)
            {
                Debug.Log("Started Crouching");
                IsCrouching = true;
                
                float NewPlayerCapsuleHeight = PlayerCapsule.height * PlayerCrouchHeightMultiplier;
               
                PlayerCapsule.height = NewPlayerCapsuleHeight;
                PlayerCapsule.center += Vector3.down * (1 - PlayerCrouchHeightMultiplier);
            }
            else if (CallbackContext.canceled)
            {
                Debug.Log("Ended crouching");
                IsCrouching = false;
                PlayerCapsule.height = DefaultCapsuleHeight;
                PlayerCapsule.center = Vector3.zero;
            }
        }
        private void ApplyGravity()
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
        
        private void ApplyMovement()
        {
            float XMovement = Velocity.x * (IsCrouching ? MovementSpeedCrouching : MovementSpeed);
            float ZMovement = Velocity.z * (IsCrouching ? MovementSpeedCrouching : MovementSpeed);
            
            PlayerRB.velocity = new Vector3(XMovement, Velocity.y, ZMovement);
        }

        private void ApplyRotation()
        {
            Vector3 Forward = UpperBody.forward;
            AngleBetweenLowerAndUpperBoddy = Vector3.Angle(Forward, LowerBody.forward);
             
            Forward = Vector3.Lerp(Forward, CurrentLookDirection, Time.deltaTime * RotationSpeed);
            UpperBody.forward = Forward;

            if (AngleBetweenLowerAndUpperBoddy > MaxAngleBetweenLowerAndUpperBody)
            {
                LowerBody.forward = Vector3.Lerp(LowerBody.forward, Forward, Time.deltaTime * RotationSpeed);
            }
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(CurrentMousePositionWorld, Vector3.one * 0.1f);
            Gizmos.DrawLine(transform.position, CurrentMousePositionWorld);
        }
    }
}

