using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed;

    private int waypointIndex;
    private float dist;

    //Vector3 lookDir;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        if (dist < 1f)
        {
            IncreaseIndex();
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void IncreaseIndex()
    {
        waypointIndex++;
        if(waypointIndex>=waypoints.Length)
        {
            waypointIndex = 0;
        }
        
        transform.LookAt(waypoints[waypointIndex].position);
        /*
        lookDir = (waypoints[waypointIndex].position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(waypoints[waypointIndex].transform.rotation, lookRotation, Time.deltaTime * 100f);
        */
    }
}
