using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUp : MonoBehaviour
{
    float distance;
    Transform target;
    float currentTime;
    bool isStop = false;
    bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * 10 * Time.deltaTime);
        if (!isStop)
        {
            currentTime += Time.deltaTime;
            distance = Vector3.Distance(target.transform.position, transform.position);
            print(distance + "wow");
            if (distance < 20)
            {
                transform.Translate(Vector3.up * 10 * Time.deltaTime);
                if(first)
                {
                    currentTime = 0;
                    first = false;
                }
                if (currentTime < 3)
                {
                    transform.Translate(Vector3.up * 10 * Time.deltaTime);
                }
            }
            if (distance < 5)
            {
                isStop = true;
            }
        }
    }
}
