using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private List<Transform> racers;
    [SerializeField] private TextMeshProUGUI rankingText; 
    [SerializeField] private Transform[] waypoints; 
    private Dictionary<Transform, int> waypointTracker = new Dictionary<Transform, int>(); 
    private Transform player; 
    private int totalRacers;

    private void Start()
    {
        totalRacers = racers.Count;
        player = racers[0]; 

        foreach (Transform racer in racers)
        {
            waypointTracker[racer] = 0; 
        }
    }

    private void Update()
    {
        UpdatePlayerRank();
    }

    private void UpdatePlayerRank()
    {
        foreach (Transform racer in racers)
        {
            UpdateWaypointForRacer(racer);
        }

        List<(Transform racer, float distance)> racerDistances = new List<(Transform, float)>();
        foreach (Transform racer in racers)
        {
            float distance = CalculateDistanceToNextWaypoint(racer);
            racerDistances.Add((racer, distance));
        }

        racerDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        int rank = 1;
        foreach (var (racer, _) in racerDistances)
        {
            if (racer == player)
            {
                break;
            }
            rank++;
        }

        rankingText.text = $"{rank} / {totalRacers}";
    }

    private void UpdateWaypointForRacer(Transform racer)
    {
        int currentWaypointIndex = waypointTracker[racer];

        for (int i = currentWaypointIndex; i < waypoints.Length; i++)
        {
            if (Vector2.Distance(racer.position, waypoints[i].position) < 2.0f)
            {
                waypointTracker[racer] = i + 1;

                if (waypointTracker[racer] >= waypoints.Length)
                {
                    waypointTracker[racer] = waypoints.Length - 1; 
                }
            }
        }
    }

    private float CalculateDistanceToNextWaypoint(Transform racer)
    {
        int currentWaypointIndex = waypointTracker[racer];
        if (currentWaypointIndex >= waypoints.Length)
        {
            return 0; 
        }
        return Vector2.Distance(racer.position, waypoints[currentWaypointIndex].position);
    }
}
