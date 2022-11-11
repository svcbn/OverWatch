using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTurret1 : MonoBehaviour
{
    float currentTime;
    public float delay;
    public GameObject TurretFactory;
    GameObject TurretOut;
    
    public Transform createPoint;

    bool finish = false;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > delay && !finish)
        {
            TurretOut = Instantiate(TurretFactory, createPoint.position, createPoint.rotation);
            finish = true;
            
        }
    }
}
