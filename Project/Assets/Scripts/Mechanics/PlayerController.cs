using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public MovementControl movementControl;
        public FlingControl flingControl;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 10;
        public Vector2 jumpTakeoff = new Vector2();

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move = new Vector2();
        bool firstMovement;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        bool charging;
        float charge = 0.5f;

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                // movement
                var moveX = Input.GetAxis("Horizontal") + movementControl.Horizontal;
                if (Mathf.Abs(moveX) < 0.2f)
                {
                    move.x = 0;
                    firstMovement = false;
                }
                else
                {
                    firstMovement = (Mathf.Abs(move.x) < 0.2f);
                    
                    // Debug.Log($"moveX = {moveX}, firstMovement = {firstMovement}");
                    move.x = moveX;
                }

                // charging
                var jumpH = Input.GetAxis("HorizontalRight") + flingControl.Horizontal;
                var jumpV = Input.GetAxis("VerticalRight") + flingControl.Vertical;

                if (Mathf.Abs(jumpH) > 0.2 || Mathf.Abs(jumpV) > 0.2)
                {
                    // Debug.Log($"H/V {jumpH}/{jumpV}");
                    charge += Time.deltaTime;
                    charging = true;
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                    float smooth = 0.8f;
                    jumpTakeoff.x = jumpTakeoff.x * smooth + jumpH * (1 - smooth);
                    jumpTakeoff.y = jumpTakeoff.y * smooth + jumpV * (1 - smooth);
                }
                else if (charging)
                {
                    jumpState = JumpState.PrepareToJump;
                    charging = false;
                }
                else
                {
                    charge = 0.5f;
                }
                //else
                //{
                //    stopJump = true;
                //    Schedule<PlayerStopJump>().player = this;
                //}
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                // start jump
                velocity = (-jumpTakeoff).normalized * Mathf.Min(charge * jumpTakeOffSpeed, jumpTakeOffSpeed * 2) * model.jumpModifier;
                // Debug.Log($"{velocity.x}/{velocity.y}");

                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y *= model.jumpDeceleration;
                    velocity.x *= model.jumpDeceleration;
                }
            }
            else if (jumpState == JumpState.Grounded || jumpState == JumpState.Landed)
            {
                // walking
                velocity.x = move.x * maxSpeed;

                animator.SetBool("grounded", IsGrounded);
                animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            }
            else if (jumpState == JumpState.InFlight && firstMovement)
            {
                // bump-drift in flight
                velocity.x += (Mathf.Sign(move.x) * maxSpeed) * 0.2f;
                animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = true;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = false;

            //targetVelocity = (move * maxSpeed);
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}