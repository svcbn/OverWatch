using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCreate : MonoBehaviour
{
    float currentTime;
    public float readyTime;

    public GameObject objectFactory;
    public Transform createPoint;

    bool finish = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //using bool create object only one time
        currentTime += Time.deltaTime;
        if(currentTime > readyTime && !finish)
        {
            Instantiate(objectFactory, createPoint.position, createPoint.rotation);
            finish = true;
        }
    }
}
