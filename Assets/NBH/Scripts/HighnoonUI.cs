using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighnoonUI : CassidyHighnoon
{
    TestEnemy Enemy;

    

    // Start is called before the first frame update
    void Start()
    {
        Enemy = GetComponent<TestEnemy>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            
            
        }
    }
}
