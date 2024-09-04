using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private AIDestinationSetter aIDestination;
    [SerializeField] private float idxLimit;
    [SerializeField] private CarController controller;
    Vector3 targetPosition = Vector3.zero;
    private int idx;

    private void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottOnBrake(inputVector.x);

        controller.SetInputVector(inputVector);

        FollowWaypoint();
    }

    private void FollowWaypoint()
    {
        targetPosition = waypoints[idx].position;
        aIDestination.target = waypoints[idx];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WaypointChild")
        {
            idx += 1;

            if (idx == idxLimit)
            {
                idx = 1;
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

    float ApplyThrottOnBrake(float inputX)
    {
        return 1.05f - Mathf.Abs(inputX) / 1.0f;
    }
}
