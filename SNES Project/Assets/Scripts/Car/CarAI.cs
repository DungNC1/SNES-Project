using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    [Header("Waypoints and AI Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private AIDestinationSetter aIDestination;
    [SerializeField] private int idxLimit;
    [SerializeField] private CarController controller;

    [Header("Aggressive Behavior Settings")]
    [SerializeField] private bool isAggressive;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float aggressiveSpeedMultiplier = 1.5f;
    [SerializeField] private Transform playerTransform;  // Reference to the player's transform
    [SerializeField] private float lateralPressureAmount = 1f;
    [SerializeField] private float lateralDecisionFrequency = 1f;
    [SerializeField] private float speedAfterCrash;

    private Vector3 targetPosition = Vector3.zero;
    private int idx;
    private bool isPushingRight = false;
    private float nextLateralDecisionTime = 0f;

    private Rigidbody rb;  // Reference to the Rigidbody for velocity checks

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Ensure the AI has a Rigidbody component
    }

    private void FixedUpdate()
    {
            // Normal AI behavior
            if (isAggressive && PlayerDetected())
            {
                ApplyPressureToPlayer();
            }
            else
            {
                FollowWaypoint();
            }

            // Calculate AI input
            Vector2 inputVector = Vector2.zero;
            inputVector.x = TurnTowardTarget();
            inputVector.y = ApplyThrottleOnBrake(inputVector.x);

            // Apply input to the car controller
            controller.SetInputVector(inputVector);
    }

    private void FollowWaypoint()
    {
        if (idx == idxLimit)
        {
            idx = 0;
        }

        targetPosition = waypoints[idx].position;
        aIDestination.target = waypoints[idx];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaypointChild"))
        {
            idx += 1;

            if (idx == idxLimit)
            {
                idx = 0;
            }
        }
    }

    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45f;
        steerAmount = Mathf.Clamp(steerAmount, -1, 1);

        return steerAmount;
    }

    float ApplyThrottleOnBrake(float inputX)
    {
        return 1.05f - Mathf.Abs(inputX);
    }

    private bool PlayerDetected()
    {
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        return distanceToPlayer <= detectionRange;
    }

    private void ApplyPressureToPlayer()
    {
        Vector3 toPlayer = playerTransform.position - transform.position;
        float forwardDot = Vector3.Dot(transform.up, toPlayer.normalized);  // Front/back relationship
        float sideDot = Vector3.Dot(transform.right, toPlayer.normalized);  // Left/right relationship

        if (Mathf.Abs(sideDot) > 0.5f && Mathf.Abs(forwardDot) < 0.5f)
        {
            if (Time.time >= nextLateralDecisionTime)
            {
                isPushingRight = sideDot < 0;  // Push right if to the left of the player
                nextLateralDecisionTime = Time.time + lateralDecisionFrequency;
            }

            Vector3 offset = isPushingRight ? Vector3.right : Vector3.left;
            offset *= lateralPressureAmount;

            targetPosition = playerTransform.position + offset;
            aIDestination.target = null;  // Temporarily disable AIDestination to manually control target
        }
        else
        {
            FollowWaypoint();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAggressive)
        {
            // Get the player's Rigidbody2D
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Calculate force to apply in the direction of the collision
                Vector2 forceDirection = collision.transform.position - transform.position;
                forceDirection.Normalize();

                // Apply force to the player's Rigidbody2D
                float forceMagnitude = 500f; // Adjust force magnitude as needed
                playerRb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);

                collision.gameObject.GetComponent<CarController>().currentSpeed = speedAfterCrash;
                StartCoroutine(ResetSpeed());
            }
        }
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(5f);
        playerTransform.GetComponent<CarController>().currentSpeed = 20;
    }
}
