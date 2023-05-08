using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : TrapBase
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float speed = 2f;

    private void Update()
    {
        transform.Rotate(0, 0, 360 * speed * Time.deltaTime);

        if (Vector3.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime *3* speed);
    }
}
