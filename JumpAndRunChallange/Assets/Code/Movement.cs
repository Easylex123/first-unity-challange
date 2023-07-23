using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Code
{
    public class Movement : MonoBehaviour
    {
        private const string HorizontalName = "Horizontal";
        private const string JumpName = "Jump";
        private const string SpeedParameterName = "Speed";
        private const string JumpHeightParameterName = "IsJumping";
        private float _horizontalMove;
        private Vector2 spawnpoint;
        private bool _isFacingRight = true;

        [Space] [SerializeField] private Rigidbody2D player;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Animator animator;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpingPower = 16f;
        private static readonly int Speed = Animator.StringToHash(SpeedParameterName);
        private static readonly int IsJumpingName  = Animator.StringToHash(JumpHeightParameterName);

        private void Start()
        {
            spawnpoint = transform.position;
        }
    
        // Update is called once per frame
        private void Update()
        {
            animator.SetFloat("ySpeed", player.velocity.y);
            _horizontalMove = Input.GetAxisRaw(HorizontalName);
            
            animator.SetFloat(Speed, Mathf.Abs(_horizontalMove));
           
            if (Input.GetButtonDown(JumpName) && IsGrounded())
            {
                animator.SetBool(IsJumpingName, true);
                player.velocity = new Vector2(player.velocity.x, _jumpingPower);
            }

            if (Input.GetButtonUp(JumpName) && player.velocity.y > 0f)
            {
                animator.SetBool(IsJumpingName, true);
                var velocity = player.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 0.5f);
                player.velocity = velocity;
            }
            
            animator.SetBool(IsJumpingName, !IsGrounded());
            Flip();
        }

        private void FixedUpdate()
        {
            player.velocity = new Vector2(_horizontalMove * _movementSpeed, player.velocity.y);
        }

        private void Flip()
        {
            if ((!_isFacingRight || !(_horizontalMove < 0f)) && (_isFacingRight || !(_horizontalMove > 0f))) return;
            _isFacingRight = !_isFacingRight;
            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale.x *= -1f;
            transform1.localScale = localScale;
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag.Equals("FallDetector"))
            {
                player.position = spawnpoint;
            }
        }
    }
}