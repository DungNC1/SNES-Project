using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float drift = 0.5f;
    [SerializeField] private float accelaration = 30f;
    [SerializeField] private float turn = 3.5f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    public float currentSpeed = 20.0f; 
    private float accelarationInput = 0;
    private float steeringInput = 0;
    private float rotaionAngle = 0;
    private float velocityUp = 0f;

    private void Update()
    {
        /*Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        SetInputVector(inputVector);*/
    }

    private void FixedUpdate()
    {
        ApplyingForce();
        ApplySteering();
        RemoveSideForce();

        if (rb.velocity.magnitude > currentSpeed)
        {
            rb.velocity = rb.velocity.normalized * currentSpeed;
        }
    }

    private void ApplyingForce()
    {
        velocityUp = Vector3.Dot(transform.up, rb.velocity);

        if (velocityUp > currentSpeed && accelarationInput > 0)
        {
            return;
        }

        if (velocityUp < -currentSpeed * 0.5f && accelarationInput < 0)
        {
            return;
        }

        if (rb.velocity.sqrMagnitude > currentSpeed * currentSpeed && accelarationInput > 0)
        {
            return;
        }

        if (accelarationInput == 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3f, Time.deltaTime * 3);
        }
        else
        {
            rb.drag = 0f;
        }

        Vector3 engineForce = transform.up * accelarationInput * accelaration;
        rb.AddForce(engineForce, ForceMode2D.Force);

        Debug.Log($"Current Speed: {currentSpeed}, Velocity: {rb.velocity.magnitude}");
    }

    private void ApplySteering()
    {
        float maxRotationAngle = (rb.velocity.magnitude / 8);
        maxRotationAngle = Mathf.Clamp01(maxRotationAngle);

        rotaionAngle -= steeringInput * turn * maxRotationAngle;
        rb.MoveRotation(rotaionAngle);
    }

    public void SetInputVector(Vector2 _inputVector)
    {
        steeringInput = _inputVector.x;
        accelarationInput = _inputVector.y;
    }

    private void RemoveSideForce()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * drift;
    }
    
    public void ModifySpeed(float amount)
    {
        currentSpeed += amount;
        currentSpeed = Mathf.Max(currentSpeed, 0f);

        Debug.Log($"Modified Speed: {currentSpeed}");
    }
}
