using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CassidyRoll : MonoBehaviour
{
    CassidyRightFire rightFire;
    CassidyRecoil recoil;

    static bool _isroll = false;
    public static bool Isroll
    {
        get
        {
            return _isroll;
        }
        set
        {
            _isroll = value;
        }
    }
 
    bool ishighnoon = false;
    static public bool isSCooldown = false;
    public static bool IsSCoolDown
    {
        get
        {
            return isSCooldown;
        }
        set
        {
            isSCooldown = value;
        }
    }

    float currentTime = 0;
    [SerializeField] float rollTime = 0.2f;
    [SerializeField] float speed = 35f;
    Vector3 dir;

    CharacterController cc;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        rightFire = GetComponent<CassidyRightFire>();
        recoil = transform.Find("CameraRotate/CameraRecoil").GetComponent<CassidyRecoil>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ishighnoon = CassidyHighnoon.ishighnoon;

        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;

        if(Input.GetKeyDown(KeyCode.LeftShift) && !_isroll && !ishighnoon && !isSCooldown)
        {
            _isroll = true;
            isSCooldown = true;
            rightFire.StopAllCoroutines();
            rightFire.isfire = false;
            StartCoroutine(Roll());
            recoil.RecoilMove(0, -50f, 0, 100f);
            recoil.RecoilRotate(0, 0, Input.GetAxisRaw("Horizontal") * -90f, 30f, 30);
        }
    }

    // 
    IEnumerator Roll()
    {
        cc.enabled = false;
        cc.enabled = true;
        CassidyMagazine.Instance.Magazine = 6;
        SoundManager.Instance.audioSources[6].Play();
        while(true)
        {
            currentTime += Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
            if(currentTime > rollTime)
            {
                currentTime = 0;
                _isroll = false;
                break;
            }  
            yield return null;  
        }
    }
}
