using System;
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

    public float currentSpeed; 
    public float originalSpeed = 0;
    [HideInInspector] public float accelarationInput = 0;
    [HideInInspector] public float steeringInput = 0;
    private float rotaionAngle = 0;
    private float velocityUp = 0f;

    [Header("Nitro")]
    [SerializeField] private float nitroDuration = 5.0f;
    [SerializeField] private float nitroSpeed = 10.0f;

    private bool isNitroActive = false;

    private void Start()
    {
        originalSpeed = currentSpeed;
    }

    private void Update()
    {
        /*Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        SetInputVector(inputVector);*/
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Nitro"))
        {
                StartCoroutine(ActivateNitro(collider.gameObject));
        }
    }

    private IEnumerator ActivateNitro(GameObject nitro)
    {
        Destroy(nitro);
        float remainingTime = nitroDuration;
        SetNitroSpeed(nitroSpeed);
        while (remainingTime > 0)
        {
            Debug.Log("nitro duration " + remainingTime);
            yield return new WaitForSeconds(1.0f);
            remainingTime -= 1.0f;
        }

        ResetSpeed();
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

        Debug.Log($"Current Speed: {currentSpeed}");
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

    public void SetNitroSpeed(float nitroSpeed)
    {
        currentSpeed += nitroSpeed;
    }
    
    public void ResetSpeed()
    {
        currentSpeed = originalSpeed; 
    }
    
}
