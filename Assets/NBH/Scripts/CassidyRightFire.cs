using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassidyRightFire : MonoBehaviour
{
    CassidyRoll roll;
    CassidyRecoil recoil;

    public GameObject bulletImpact1;
    public GameObject bulletImpact2;
    public GameObject bulletImpact3;
    public GameObject bulletImapct4;
    public GameObject muzzleFlash;
    public GameObject bulletFactory;

    GameObject firePos;
    GameObject bodyHit;

    public Animation anim;

    public LayerMask playerMask;

    Vector3 shootdir;

    int bulletCount;
    bool isreload { get; set; }
    bool isroll { get; set; }
    bool ishighnoon { get; set; }
    public bool isfire { get; set; }

    float randx;
    float randy;

    // Start is called before the first frame update
    void Start()
    {
        roll = GetComponent<CassidyRoll>();
        recoil = transform.Find("CameraRotate/CameraRecoil").GetComponent<CassidyRecoil>();
        playerMask = LayerMask.GetMask("Player");
        firePos = GameObject.Find("FirePos");
        bodyHit = (GameObject)Resources.Load("BodyHit");

    }

    // Update is called once per frame
    void Update()
    {
        bulletCount = CassidyMagazine.Instance.Magazine;
        isreload = CassidyMagazine.Instance.isreload;
        ishighnoon = CassidyHighnoon.ishighnoon;
        isroll = CassidyRoll.Isroll;
        
        randx = Random.Range(-0.1f, 0.1f);
        randy = Random.Range(-0.05f, 0.05f);
        shootdir = Camera.main.transform.forward + new Vector3(randx, randy, randx);



        if (bulletCount > 0)
        {
            if (Input.GetButtonDown("Fire2") && !isreload && !isfire && !isroll && !ishighnoon)
            {
                isfire = true;
                anim.Stop();
                anim.Play();
                StartCoroutine(WildlyFire());
            }
        }
    }

    public IEnumerator WildlyFire()
    {
        SoundManager.Instance.audioSources[3].Play();
        while (bulletCount > 0)
        {
            Ray ray = new Ray(Camera.main.transform.position, shootdir);
            RaycastHit hitInfo;
            recoil.RecoilRotate(-120, 0, 20, 20);
            if (Physics.Raycast(ray, out hitInfo, 1000, ~playerMask))
            {
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
                bullet.transform.forward = (hitInfo.point - firePos.transform.position);
                mf.transform.position = firePos.transform.position;

                Destroy(bI1, 5f);
                Destroy(bI2, 1f);
                Destroy(bI3, 1f);
                Destroy(bI4, 3f);
                Destroy(bullet, 3f);
                Destroy(mf, 0.2f);

                TestEnemy testEnemy = hitInfo.transform.GetComponent<TestEnemy>();
                if (testEnemy)
                {
                    //피격 이벤트를 호출
                    //enemy.OnDamageProcess(hitInfo.point);
                    //GameObject bh = Instantiate(bodyHit, Camera.main.WorldToScreenPoint(Camera.main.transform.forward), Quaternion.identity, GameObject.Find("Canvas").transform);
                    GameObject bh = Instantiate(bodyHit, GameObject.Find("Canvas").transform);
                    Destroy(bh, 0.1f);

                }
                MoveTorbjorn Torbjorn = hitInfo.transform.GetComponent<MoveTorbjorn>();
                if (Torbjorn)
                {
                    Torbjorn.OnDamageProcess(ray.direction);
                }
            }

           

            CassidyMagazine.Instance.Magazine--;
            yield return new WaitForSeconds(0.13f);
        }
        isfire = false;
        StartCoroutine(CassidyMagazine.Instance.Reload());
    }
}
