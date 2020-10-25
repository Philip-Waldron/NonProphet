using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    public float Speed = 12;
    public float Gravity = -9.81f;
    public float JumpHeight = 3f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    public Rigidbody PlayerBody;

    private bool _startXMovement;
    private bool _startYMovement;

    private Vector3 _velocity;
    private Vector3 _lastPosition;
    private bool _isGrounded;

    void Update()
    {
        Move();
        ApplyGravity();
    }

    private void Move()
    {
        // Check if player is grounded.
        _isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        // Get movement inputs.
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        ApplyMovementBallistics2(xInput, yInput);

        // Move the player in horizontal directions.
        Vector3 move = transform.right * xInput + transform.forward * yInput;
        Controller.Move(Time.deltaTime * Speed * move);
    }

    private void ApplyMovementBallistics(float xInput, float yInput)
    {
        if (xInput > 0.5)
        {
            if (_startXMovement == false)
            {
                _startXMovement = true;
                PlayerBody.AddRelativeForce(Vector3.forward * 1000);
            }
        }
        else if (xInput < -0.5)
        {
            if (_startXMovement == false)
            {
                _startXMovement = true;
                PlayerBody.AddRelativeForce(Vector3.back * 1000);
            }
        }
        else
        {
            _startXMovement = false;
        }

        if (yInput > 0.5)
        {
            if (_startYMovement == false)
            {
                _startYMovement = true;
                PlayerBody.AddRelativeForce(Vector3.right * 1000);
            }
        }
        else if (yInput < -0.5)
        {
            if (_startYMovement == false)
            {
                _startYMovement = true;
                PlayerBody.AddRelativeForce(Vector3.left * 1000);
            }
        }
        else
        {
            _startYMovement = false;
        }
    }

    private void ApplyMovementBallistics2(float xInput, float yInput)
    {
        PlayerBody.AddRelativeForce(Time.deltaTime * xInput * 5000 * Vector3.forward);
        PlayerBody.AddRelativeForce(Time.deltaTime * yInput * 5000 * Vector3.right);
    }

    private void ApplyGravity()
    {
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2;

            if (Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Sqrt(JumpHeight * -2 * Gravity);
            }
        }

        _lastPosition = transform.position;

        // Apply gravity/jump velocity to the player.
        _velocity.y += Gravity * Time.deltaTime;
        Controller.Move(_velocity * Time.deltaTime);
    }
}
