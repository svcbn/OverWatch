using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]

public class TestEnemy : MonoBehaviour
{
    int hp = 200;
    
    [SerializeField] float exposedTime = 0;

    public float ExposedTime
    {
        get
        {
            return exposedTime;
        }
        set
        {
            exposedTime = value;
        }
    }
    CassidyHighnoon highnoon;


    // Start is called before the first frame update
    void Start()
    {
        highnoon = CassidyHighnoon.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
