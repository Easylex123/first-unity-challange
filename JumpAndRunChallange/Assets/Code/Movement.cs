using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code
{
    public class Movement : MonoBehaviour
    {
        private const string HorizontalName = "Horizontal";
        private const string JumpName = "Jump";
        private const string SpeedParameterName = "Speed";
        private float _horizontalMove;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpingPower = 16f;
        private bool _isFacingRight = true;

        [Space] [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Animator animator;
        private static readonly int Speed = Animator.StringToHash(SpeedParameterName);


        // Update is called once per frame
        private void Update()
        {
            _horizontalMove = Input.GetAxisRaw(HorizontalName);
            
            animator.SetFloat(Speed, Mathf.Abs(_horizontalMove));
           
            if (Input.GetButtonDown(JumpName) && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, _jumpingPower);
            }

            if (Input.GetButtonUp(JumpName) && rb.velocity.y > 0f)
            {
                var velocity = rb.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 0.5f);
                rb.velocity = velocity;
            }
            
            Flip();
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(_horizontalMove * _movementSpeed, rb.velocity.y);
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
    }
}