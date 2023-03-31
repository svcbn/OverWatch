using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CassidyLeftFire : MonoBehaviour
{
    CassidyRightFire rightFire;
    CassidyRoll roll;
    CassidyRecoil recoil;
    RevolverRecoil revolverRecoil;

    public GameObject bulletImpact1;
    public GameObject bulletImpact2;
    public GameObject bulletImpact3;
    public GameObject bulletImapct4;
    public GameObject muzzleFlash;
    public GameObject bulletFactory;

    public LayerMask playerMask;

    GameObject firePos;
    GameObject bodyHit;
    GameObject headHit;

    public Animation gunRecoilAnim;

    //AudioSource bulletAudio;
    int bulletCount;
    float currentTime = 0;
    float delayTime = 0.5f;


    public bool isdelay = false;
    bool isreload = false;
    bool isfire = false;
    bool isroll = false;
    bool ishighnoon = false;

    // Start is called before the first frame update
    void Start()
    {
        //bulletPS = bulletImpact.GetComponent<ParticleSystem>();
        //bulletAudio = bulletImpact.GetComponent<AudioSource>();
        rightFire = GetComponent<CassidyRightFire>();
        revolverRecoil = GetComponentInChildren<RevolverRecoil>();

        recoil = transform.Find("CameraRotate/CameraRecoil").GetComponent<CassidyRecoil>();
        firePos = GameObject.Find("FirePos");
        playerMask = LayerMask.GetMask("Player");
        bodyHit = (GameObject)Resources.Load("BodyHit");
        headHit = (GameObject)Resources.Load("HeadHit");
    }

    // Update is called once per frame
    void Update()
    {
        isfire = rightFire.isfire;
        isroll = CassidyRoll.Isroll;
        isreload = CassidyMagazine.Instance.isreload;
        bulletCount = CassidyMagazine.Instance.Magazine;
        ishighnoon = CassidyHighnoon.ishighnoon;

        if (bulletCount > 0)
        {
            if (Input.GetButton("Fire1") && !isdelay && !isreload && !isfire && !isroll && !ishighnoon)
            {
                //bulletAudio.Stop();
                //bulletAudio.Play();
                Fire();
                recoil.RecoilRotate(-80, 0, 0, 30);
                recoil.RecoilMove(0, 0, -40, 80);
                //revolverRecoil.RecoilRotate(-150, 0, 0, 60);
                //revolverRecoil.RecoilMove(0, 0, -200, 100);

                CassidyMagazine.Instance.Magazine--;
                isdelay = true;

            }
        }
        
        if (bulletCount < 1 && !isreload)
        {
            isreload = true;
            StartCoroutine(CassidyMagazine.Instance.Reload());

        }

        if(isdelay)
        {
            currentTime += Time.deltaTime;
            if(currentTime > delayTime)
            {
                isdelay = false;
                currentTime = 0;
            }
        }

    }

    public void Fire()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1000, ~playerMask))
        {
            gunRecoilAnim.Stop();
            gunRecoilAnim.Play();

            GameObject bI1 = Instantiate(bulletImpact1);
            GameObject bI2 = Instantiate(bulletImpact2);
            GameObject bI3 = Instantiate(bulletImpact3);
            GameObject bI4 = Instantiate(bulletImapct4);
            GameObject bullet = Instantiate(bulletFactory);
            GameObject mf = Instantiate(muzzleFlash);

            bI1.transform.position = hitInfo.point;
            bI1.transform.forward = hitInfo.normal;
            bI2.transform.position = hitInfo.point;
            bI2.transform.forward = hitInfo.normal;
            bI3.transform.position = hitInfo.point;
            bI3.transform.forward = -hitInfo.normal;
            bI4.transform.position = hitInfo.point;
            bI4.transform.forward = hitInfo.normal;
            bullet.transform.position = firePos.transform.position;
            bullet.transform.forward = hitInfo.point - firePos.transform.position;
            mf.transform.position = firePos.transform.position;


            Destroy(bI1, 5f);
            Destroy(bI2, 1f);
            Destroy(bI3, 1f);
            Destroy(bI4, 3f);
            Destroy(bullet, 3f);
            Destroy(mf, 0.2f);


            EnemyHead enemyHead = hitInfo.transform.GetComponent<EnemyHead>();
            
            if (enemyHead)
            {
                GameObject headHitMark = Instantiate(headHit, GameObject.Find("Canvas").transform);
                headHitMark.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                SoundManager.Instance.audioSources[2].Play();
            }
            else
            {
                //만약 맞은 녀석이 Enemy라면
                TestEnemy testEnemy = hitInfo.transform.GetComponent<TestEnemy>();
                if (testEnemy)
                {
                    //피격 이벤트를 호출
                    //enemy.OnDamageProcess(hitInfo.point);

                    GameObject bodyHitMark = Instantiate(bodyHit, GameObject.Find("Canvas").transform);
                    bodyHitMark.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                    SoundManager.Instance.audioSources[1].Play();
                }
                else
                {
                    SoundManager.Instance.audioSources[0].Play();
                }
            }

            MoveTorbjorn Torbjorn = hitInfo.transform.GetComponent<MoveTorbjorn>();
            if (Torbjorn)
            {
                Torbjorn.OnDamageProcess(ray.direction);
            }
        }
    }
}
