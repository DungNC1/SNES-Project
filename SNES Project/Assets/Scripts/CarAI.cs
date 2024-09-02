using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    private struct structAI
    {
        public Transform checkpoints;
        public int idx;
        public Vector3 directionSteer;
        public Quaternion rotationSteer;
    }

    private structAI ai;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private CarController carController;
    Vector3 targetPosition = Vector3.zero;

    private void Start()
    {
        ai.checkpoints = GameObject.FindWithTag("Waypoint").transform;
        ai.idx = 0;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottOnBrake(inputVector.x);

        carController.SetInputVector(inputVector);

        FollowWaypoint();
    }

    private void FollowWaypoint()
    {
        targetPosition = wayPoints[ai.idx].position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaypointChild") == true)
        {
            ai.idx = CalcNextCheckpoint();
        }
    }

    private int CalcNextCheckpoint()
    {
        int curr = ExtractNumberFromString(ai.checkpoints.GetChild(ai.idx).name);
        int next = curr + 1;
        if (next > ai.checkpoints.childCount - 1)
            next = 0;

        Debug.Log(string.Format("current checkpoint {0}, next {1}", curr, next));

        return next;
    }

    private int ExtractNumberFromString(string s1)
    {
        return System.Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(s1, "[^0-9]", ""));
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
