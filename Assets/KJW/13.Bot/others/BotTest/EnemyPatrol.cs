using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float MovementSpeed = 3f;
    public float TurningSpeed = 3f;

    Vector3 dist;
    bool WpReached;
    GameObject StartingPoint;
    string TargetWpToGo;
    int CurrentWpNumber;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartingPoint = FindClosestWaypoint();
        TargetWpToGo = StartingPoint.gameObject.name;
        CurrentWpNumber = int.Parse(TargetWpToGo.Split(char.Parse("-"))[1]);
        Debug.Log("Current Wp Target : " + CurrentWpNumber);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        GameObject WpToGo = GameObject.Find("wp-" + CurrentWpNumber);
        Vector3 lookPos = WpToGo.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurningSpeed);
        transform.position += transform.forward * Time.deltaTime * MovementSpeed;
    }

    //Function for finding closest waypoint
    public GameObject FindClosestWaypoint()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Waypoint");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint" && !WpReached)
        {
            WpReached = true;
            if (GameObject.Find("wp-" + (CurrentWpNumber + 1)) != null)
                CurrentWpNumber += 1;
            else
                CurrentWpNumber = 0;
        }
        Debug.Log("Current Wp Target : " + CurrentWpNumber);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Waypoint" && WpReached)
        {
            WpReached = false;
        }
    }
}