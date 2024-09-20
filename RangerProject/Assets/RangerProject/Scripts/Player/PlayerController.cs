using System;
using RangerProject.Scripts.Enviroment;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace RangerProject.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float MovementSpeed = 10.0f;
        [SerializeField] private float RotationSpeed = 1.0f;
        [SerializeField] private float MovementSpeedCrouching = 10.0f;
      
        [SerializeField] private float Gravity = -9.81f;
        [SerializeField] private float GravityMultiplier = 1.0f;
        [SerializeField] private float JumpForce = 10.0f;
        [SerializeField] private float MaxAngleBetweenLowerAndUpperBody = 20.0f;
        [SerializeField] private float MinAimableRange = 0.5f;
        [SerializeField, Range(0, 1.0f)] private float PlayerCrouchHeightMultiplier = 0.7f;
        [SerializeField, Range(0, 10.0f)] private float AimHeightOffset = 2.0f;
        [SerializeField] private string GroundTag = "Ground";
        [SerializeField] private LayerMask AimAbleLayer;
        [SerializeField] private Transform AimTarget;
        [SerializeField] private CapsuleCollider PlayerCapsule;
        [SerializeField] private PlayerInput PlayerInput;
        [SerializeField] private Rigidbody PlayerRB;
        [SerializeField] private Animator PlayerAnimator;

        private float CurrentAngleBetweenUpperAndLowerBody = 0.0f;
        private float DefaultCapsuleHeight = 0.0f;
        private bool IsGrounded = false;
        private bool IsCrouching = false;
        private bool IsIdle = false;
        private bool IsWalking = false;
        private bool IsJumping = false;
        private bool IsFalling = false;
        private Vector3 Velocity = Vector3.zero;
        private Vector3 VelocityWithoutY = Vector3.zero;
        private Vector3 CurrentMousePositionWorld = Vector3.zero;
        private Vector3 CurrentLookDirection = new Vector3(0,0, 1);

        private int JumpingUpId = Animator.StringToHash("IsJumping");
        private int JumpingDownId = Animator.StringToHash("IsFalling");
        private int IdleId = Animator.StringToHash("IsIdle");
        private int WalkingId = Animator.StringToHash("IsWalking");
        private int BlendXId = Animator.StringToHash("BlendX");
        private int BlendYId = Animator.StringToHash("BlendY");
        public Vector3 GetCurrentMousePositionWorld() => CurrentMousePositionWorld;
        
        private void Start()
        {
            PlayerInput.camera = Camera.main;
            DefaultCapsuleHeight = PlayerCapsule.height;
            AimTarget.SetParent(null, true);
        }

        private void Update()
        {
            Aim();
            ApplyRotation();
            EvaluateState();
            SetBlendValuesLowerBody();
            
            ApplyGravity();
            ApplyMovement();
        }

        private void FixedUpdate()
        {
           
        }

        public void Aim()
        {
            Ray MousePositionRay = PlayerInput.camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(MousePositionRay, out RaycastHit Hit, Mathf.Infinity, AimAbleLayer);
            CurrentMousePositionWorld = Hit.point;
        }
        public void OnJump(InputAction.CallbackContext CallbackContext)
        {
            if (CallbackContext.started && IsGrounded)
            {
                IsGrounded = false;
                IsJumping = true;
                Velocity.y += JumpForce;
            }
        }
        public void OnWalk(InputAction.CallbackContext CallbackContext)
        {
            Vector2 WalkInput = CallbackContext.ReadValue<Vector2>();

            float XMovement = WalkInput.x;
            float ZMovement = WalkInput.y;
            
            Velocity = new Vector3(XMovement, Velocity.y, ZMovement);
            VelocityWithoutY = new Vector3(Velocity.x, 0, Velocity.z);
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

        void EvaluateState()
        {
            if (IsGrounded)
            {
                if (VelocityWithoutY.Equals(Vector3.zero))
                {
                    IsIdle = true;
                    IsWalking = false;
                    IsFalling = false;
                    PlayerAnimator.SetBool(JumpingDownId, IsFalling);
                    PlayerAnimator.SetBool(IdleId, IsIdle);                    
                    PlayerAnimator.SetBool(WalkingId, IsWalking);
                    PlayerAnimator.SetFloat(BlendXId, 0.0f);
                    PlayerAnimator.SetFloat(BlendYId, 0.0f);
                }
                else
                {
                    IsIdle = false;
                    IsWalking = true;
                    PlayerAnimator.SetBool(IdleId, IsIdle);
                    PlayerAnimator.SetBool(WalkingId, IsWalking);
                }
            }
            else if (!IsGrounded)
            {
                if (Velocity.y < 0)
                {
                    IsFalling = true;
                    IsJumping = false;
                    PlayerAnimator.SetBool(JumpingDownId, IsFalling);
                    PlayerAnimator.SetBool(JumpingUpId, IsJumping);
                }
                else if(Velocity.y > 0)
                {
                    IsJumping = true;
                    IsFalling = false;
                    PlayerAnimator.SetBool(JumpingUpId, IsJumping);
                    PlayerAnimator.SetBool(JumpingDownId, IsFalling);
                }
                
                IsIdle = false;
                IsWalking = false;
            }
        }

        void SetBlendValuesLowerBody()
        {
            if (Velocity.Equals(Vector3.zero) || !IsGrounded)
            {
                PlayerAnimator.SetFloat(BlendXId, 0.0f);
                PlayerAnimator.SetFloat(BlendYId, 0.0f);
                
                return;
            }

            Vector3 VelocityRelativeToPlayer = transform.InverseTransformDirection(VelocityWithoutY);
            
            PlayerAnimator.SetFloat(BlendXId, VelocityRelativeToPlayer.x);
            PlayerAnimator.SetFloat(BlendYId, VelocityRelativeToPlayer.z);
        }
        
        private void ApplyGravity()
        {
            if (IsGrounded && Velocity.y < 0)
            {
                Velocity.y = 0.0f;
            }
            else if(!IsGrounded)
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
            var PlayerPosition = transform.position;
            Vector3 DirectionToMouse = CurrentMousePositionWorld - PlayerPosition;
            DirectionToMouse.y = 0;
            CurrentLookDirection = DirectionToMouse.normalized;

            Vector3 PlayerPositionWithoutY = new Vector3(PlayerPosition.x, 0, PlayerPosition.z);
            Vector3 MousePositionWorldWithoutY = new Vector3(CurrentMousePositionWorld.x, 0, CurrentMousePositionWorld.z);
            
            //if the distance to the current mouse pos is to low clamp the positionso that we dont aim into ourselves
            if (Vector3.Distance(PlayerPositionWithoutY, MousePositionWorldWithoutY) <= MinAimableRange)
            {
                Vector3 DirectionToMousePosWithoutY = (MousePositionWorldWithoutY - PlayerPositionWithoutY).normalized;
                Vector3 ClampedDirection = DirectionToMousePosWithoutY  * MinAimableRange;
                ClampedDirection.y = CurrentMousePositionWorld.y;
                
                AimTarget.position = transform.position + new Vector3(ClampedDirection.x,  (CurrentMousePositionWorld.y + AimHeightOffset) - transform.position.y, ClampedDirection.z);
            }
            else
            {
                AimTarget.position = Vector3.Lerp(AimTarget.position, CurrentMousePositionWorld + new Vector3(0, AimHeightOffset, 0), Time.deltaTime * RotationSpeed);
            }
            
            CurrentAngleBetweenUpperAndLowerBody =
                Mathf.Acos(Vector3.Dot(transform.forward, CurrentLookDirection)) * Mathf.Rad2Deg;
            
            if (CurrentAngleBetweenUpperAndLowerBody > MaxAngleBetweenLowerAndUpperBody)
            {
                transform.forward = Vector3.Lerp(transform.forward, CurrentLookDirection, Time.deltaTime * RotationSpeed);
            }
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(GroundTag))
            {
                IsGrounded = true;
                IsJumping = false;
                IsFalling = false;
                
                PlayerAnimator.SetBool(JumpingUpId, false);
                PlayerAnimator.SetBool(JumpingDownId, false);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(CurrentMousePositionWorld, Vector3.one * 0.1f);
            Gizmos.DrawSphere(AimTarget.position, 0.1f);
            Gizmos.DrawLine(transform.position, CurrentMousePositionWorld);
        }
    }
}

