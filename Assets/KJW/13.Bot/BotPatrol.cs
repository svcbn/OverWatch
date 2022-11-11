using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform botBody;
    private int speed = 40;

    private int waypointIndex;
    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        botBody.transform.LookAt(waypoints[waypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(botBody.transform.position, waypoints[waypointIndex].position);
        if (dist < 1f)
        {
            IncreaseIndex();
        }
        botBody.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void IncreaseIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }

        botBody.transform.LookAt(waypoints[waypointIndex].position);
    }
}
