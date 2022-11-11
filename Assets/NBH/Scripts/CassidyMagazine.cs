using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CassidyMagazine : MonoBehaviour
{
    public static CassidyMagazine Instance;
    CassidyLeftFire leftFire;
    CassidyRightFire rightFire;
    CassidyRecoil recoil;

    public Text magazineText;

    int currentBullet = 6;

    float currentTime = 0;

    public bool isreload = false;
    

    public int Magazine
    {
        get
        {
            return currentBullet;
        }
        set
        {
            currentBullet = value;
            magazineText.text = currentBullet + "";
        }
    }


    private void Awake()
    {
        Instance = this;
        leftFire = GetComponent<CassidyLeftFire>();
        rightFire = GetComponent<CassidyRightFire>();
    }

    // Start is called before the first frame update
    void Start()
    {
        magazineText.text = currentBullet + "";
        recoil = transform.Find("CameraRotate/CameraRecoil").GetComponent<CassidyRecoil>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentBullet < 6 && !isreload)
        {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        //recoil.RecoilRotate(-10, 10, 0, 10);
        SoundManager.Instance.audioSources[5].Play();
        isreload = true;
        yield return new WaitForSeconds(0.5f);
        
        if(Magazine == 6)
        {
            isreload = false;
            yield break;
        }

        yield return new WaitForSeconds(1.0f);

        Magazine = 6;
        isreload = false;
        
    }
}
