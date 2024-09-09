using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public enum AIMode
    {
        Friendly,
        Agressive,
        None
    }

    [Header("Waypoints and AI Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int idxLimit;
    [SerializeField] private CarController controller;

    [Header("Behaviour Settings")]
    [SerializeField] private AIMode aiMode;

    [Header("Aggressive Behavior Settings")]
    [SerializeField] private bool isAggressive;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private Transform playerTransform;  // Reference to the player's transform
    [SerializeField] private float lateralPressureAmount = 1f;
    [SerializeField] private float lateralDecisionFrequency = 1f;
    [SerializeField] private float speedAfterCrash;
    [SerializeField] private float minTimeBeforeCheckingCollision = 0.85f;

    [Header("Friendly Behavior Settings")]
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("Coin Settings")]
    [SerializeField] private float coinDetectionRange = 8f;  // Range at which AI can detect coins
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private LayerMask coinLayerMask;  // Layer for coins

    [Header("Effects")]
    [SerializeField] private Transform cam;

    private Vector3 targetPosition = Vector3.zero;
    private bool isPushingRight = false;
    private float nextLateralDecisionTime = 0f;
    private bool isColliding;
    private int idx;

    private Transform currentCoinTarget;


    private void FixedUpdate()
    {
        if (CoinDetected(out Transform coinTransform) && !isPlayer)
        {
            // If a coin is detected, prioritize it
            currentCoinTarget = coinTransform;
            targetPosition = currentCoinTarget.position;
        }
        else
        {
            // If no coin is detected, proceed with normal behavior
            currentCoinTarget = null;

            switch (aiMode)
            {
                case AIMode.None:
                    isAggressive = false;
                    FollowWaypoint();
                    break;

                case AIMode.Agressive:
                    isAggressive = true;

                    if (PlayerDetected() && idx != 0)
                    {
                        ApplyPressureToPlayer();
                    }
                    else
                    {
                        FollowWaypoint();
                    }
                    break;

                case AIMode.Friendly:
                    FollowWaypoint();
                    break;
            }
        }


        // Calculate AI input
        Vector2 inputVector = Vector2.zero;
        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOnBrake(inputVector.x);

        // Apply input to the car controller
        controller.SetInputVector(inputVector);
        targetPosition = waypoints[idx].position;
    }

    private void FollowWaypoint()
    {
        if (idx == idxLimit)
        {
            cam.GetComponent<CameraMovement>().cameraState = CameraMovement.CameraState.Free;
            cam.position = new Vector3(transform.position.x, transform.position.y, -10);
            idx = 0;
        }
    }

    private bool CoinDetected(out Transform coinTransform)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, coinDetectionRange, coinLayerMask);

        if (hits.Length > 0)
        {
            // Prioritize the closest coin
            Transform closestCoin = hits[0].transform;
            float minDistance = Vector2.Distance(transform.position, closestCoin.position);

            foreach (var hit in hits)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCoin = hit.transform;
                }
            }

            coinTransform = closestCoin;
            return true;
        }

        coinTransform = null;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("WaypointChild"))
        {
            if(!isColliding)
            {
                isColliding = true;
                idx++;
                StartCoroutine(ResetCollision(minTimeBeforeCheckingCollision));
            } 

            if(idx == idxLimit)
            {

            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            // Handle coin collection (destroy the coin, add points, etc.)
            Destroy(collision.gameObject);
            Debug.Log($"{gameObject.name} collected a coin!");
        }
    }


    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        if(AIMode.Friendly == aiMode)
        {
            isAggressive = false;

            AvoidCar(vectorToTarget, out vectorToTarget);
        }

        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45f;
        steerAmount = Mathf.Clamp(steerAmount, -1, 1);

        return steerAmount;
    }

    private void AvoidCar(Vector2 vectorToTarget, out Vector2 newVectorToTarget)
    {
        if(CarDetection(out Vector3 otherCarPosition, out Vector3 otherCarRightVector))
        {
            Vector2 avoidanceVector = Vector2.zero;
            avoidanceVector = Vector2.Reflect((otherCarPosition - transform.position).normalized, otherCarRightVector);

            float distanceToTarget = (targetPosition - transform.position).magnitude;
            float driveToTargetInfluence = 6f / distanceToTarget;

            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.3f, 1f);

            float avoidanceInInfuence = 1f - driveToTargetInfluence;

            newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVector * avoidanceInInfuence;
            newVectorToTarget.Normalize();

            return;
        }

        newVectorToTarget = vectorToTarget;
    }

    private bool CarDetection(out Vector3 position, out Vector3 otherCarRightVector)
    {
        //boxCollider2D.enabled = false;
        RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, 1 << LayerMask.NameToLayer("AI"));
        //boxCollider2D.enabled = true;

        if (raycastHit2D.collider != null)
        {
            position = raycastHit2D.collider.transform.position;
            otherCarRightVector = raycastHit2D.collider.transform.right;

            return true;
        }

        position = Vector3.zero;
        otherCarRightVector = Vector3.zero;

        return false;
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
        // Decide whether to move left or right to apply pressure
        if (Time.time >= nextLateralDecisionTime)
        {
            // Randomly decide to push left or right
            isPushingRight = Random.value > 0.5f;
            nextLateralDecisionTime = Time.time + lateralDecisionFrequency;
        }

        // Get the player's position and calculate a target offset to apply pressure
        Vector3 offset = isPushingRight ? Vector3.right : Vector3.left;
        offset *= lateralPressureAmount;

        // Set the AI's target to the player's position, adjusted with the lateral offset
        targetPosition = playerTransform.position + offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");

        if (collision.gameObject.CompareTag("Player") && isAggressive)
        {
            // Get the player's Rigidbody2D
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Calculate force to apply in the direction of the collision
                Vector2 forceDirection = collision.transform.position - transform.position;
                forceDirection.Normalize();

                Debug.Log("Collision2");

                // Apply force to the player's Rigidbody2D
                float forceMagnitude = 1000f; // Adjust force magnitude as needed
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

    private IEnumerator ResetCollision(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isColliding = false;
    }
}
