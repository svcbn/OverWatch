using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassidyStunGranade : MonoBehaviour
{
    GameObject stunGranade;
    Camera cam;
    public static bool _isECooldown = false;
    public static bool IsECooldown
    {
        get
        {
            return _isECooldown;
        }
        set
        {
            _isECooldown = value;
        }
    }

    CassidySkillUI skill;

    // Start is called before the first frame update

    void Start()
    {
        stunGranade = (GameObject)Resources.Load("StunGranade");
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.E) && !_isECooldown && !CassidyHighnoon.ishighnoon)
        {
            _isECooldown = true;
            Throw();
        }
    }

    void Throw()
    {
        GameObject SG = Instantiate(stunGranade);
        SG.transform.position = cam.transform.position + cam.transform.up * -0.3f + cam.transform.right * -0.15f;
    }
}
