using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// 적으로 판별 되면 그 게임오브젝트 리스트에 집어넣고
// 적으로 판별(?)된 물체에 타겟포인트 점점 줄어듬
// 시간이 지나면 최대치가 되고 데미지가 증가 최대6초 초당 170증가 1.5초부터 초당 550증가
// 시간제한이 흘러감
// 좌클릭시 현재 타겟중 오른쪽부터 발사 0.167초 딜레이
// 스킬 비활성화 ui



public class CassidyHighnoon : MonoBehaviour
{
    PlayerMove pm;
    CassidyLeftFire leftFire;
    CassidyRecoil recoil;

    Camera cam;
    float currentTime = 0;
    float highnoonTime = 6f;

    public static bool ishighnoon = false;
    public Image highnoonBG;
    Color alpha;
    public static CassidyHighnoon Instance;

    public Collider[] enemies;
    public List<GameObject> targetInScreen;

    Transform originPos;
    GameObject camRotate;

    GameObject redCircle;
    GameObject bodyHit;
    GameObject deathMark;

    public Text LeftText;
    public Text RightText;
    public Text Second;
    public Text SecondsLeft;
    public Image LeftMouse;
    public Image rightMouse;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    GameObject bulletImpact1;
    GameObject bulletImpact2;
    GameObject bulletImpact3;
    GameObject bulletImpact4;
    GameObject muzzleFlash;
    GameObject bulletFactory;

    GameObject firePos;

    public Animation gunRecoilAnim;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMove>();
        leftFire = GetComponent<CassidyLeftFire>();
        cam = Camera.main;
        camRotate = GameObject.Find("CameraRotate");
        alpha = highnoonBG.color;
        targetMask = LayerMask.GetMask("Enemy");
        obstacleMask = LayerMask.GetMask("Map");
        playerMask = LayerMask.GetMask("Player");
        firePos = GameObject.Find("FirePos");
        recoil = transform.Find("CameraRotate/CameraRecoil").GetComponent<CassidyRecoil>();


        bulletImpact1 = (GameObject)Resources.Load("BulletImpactFlesh");
        bulletImpact2 = (GameObject)Resources.Load("BulletImpactSmoke");
        bulletImpact3 = (GameObject)Resources.Load("BulletSparkO");
        bulletImpact4 = (GameObject)Resources.Load("BulletLeftover");
        muzzleFlash = (GameObject)Resources.Load("MuzzleFlash");
        bulletFactory = (GameObject)Resources.Load("GhostBullet");

        redCircle = (GameObject)Resources.Load("RedCircle");
        bodyHit = (GameObject)Resources.Load("BodyHit");
        deathMark = (GameObject)Resources.Load("DeathMark");

        LeftText.color = new Color(1, 1, 1, 0);
        RightText.color = new Color(1, 1, 1, 0);
        LeftMouse.color = new Color(1, 1, 1, 0);
        rightMouse.color = new Color(1, 1, 1, 0);
        Second.color = new Color(1, 1, 1, 0);
        SecondsLeft.color = new Color(1, 1, 1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        enemies = Physics.OverlapSphere(transform.position, 50f, targetMask);
        originPos = camRotate.transform;
        SecondsLeft.text = Mathf.Ceil(highnoonTime - currentTime) + "";

        if (Input.GetKeyDown(KeyCode.Q) && !ishighnoon)
        {
            StartCoroutine(ItsHighNoon());
            StartCoroutine(FindTargets());
        }

    }

    IEnumerator ItsHighNoon()
    {
        ishighnoon = true;
        pm.speed = 5 * 0.35f;
        CassidyMagazine.Instance.Magazine = 6;
        float camTime = 0;

        LeftText.color = new Color(1, 1, 1, 0.7843f);
        RightText.color = new Color(1, 1, 1, 0.7843f);
        LeftMouse.color = new Color(1, 1, 1, 0.7843f);
        rightMouse.color = new Color(1, 1, 1, 0.7843f);
        Second.color = new Color(1, 1, 1, 0.7843f);
        SecondsLeft.color = new Color(1, 1, 1, 0.7843f);
        SoundManager.Instance.audioSources[9].Play();


        while (true)
        {
            highnoonBG.color = alpha;
            currentTime += Time.deltaTime;

            if (currentTime < 0.3f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, cam.transform.position - cam.transform.forward * 0.2f, 4 * Time.deltaTime);
                alpha.a = Mathf.Lerp(0, 0.2352f, 5 * currentTime);

            }


            if (Input.GetButtonDown("Fire1"))
            {
                SoundManager.Instance.audioSources[9].Stop();
                StartCoroutine(HighnoonFire());
                ishighnoon = false;
            }
            if (Input.GetButtonDown("Fire2") && targetInScreen.Count > 0)
            {
                ishighnoon = false;
                SoundManager.Instance.audioSources[9].Stop();
            }

            if (currentTime > highnoonTime)
            {
                ishighnoon = false;
            }

            if (!ishighnoon)
            {
                camTime += Time.deltaTime;
                if (camTime < 0.3f)
                {
                    cam.transform.position = Vector3.Lerp(cam.transform.position, originPos.position, 4 * Time.deltaTime);
                    alpha.a = Mathf.Lerp(0.2352f, 0, 5 * camTime);

                }
                else
                {
                    currentTime = 0;
                    break;
                }
            }

            yield return null;
        }

        LeftText.color = new Color(1, 1, 1, 0);
        RightText.color = new Color(1, 1, 1, 0);
        LeftMouse.color = new Color(1, 1, 1, 0);
        rightMouse.color = new Color(1, 1, 1, 0);
        Second.color = new Color(1, 1, 1, 0);
        SecondsLeft.color = new Color(1, 1, 1, 0);
        targetInScreen.Clear();
        pm.speed = 5;
    }


    IEnumerator FindTargets()
    {
        List<GameObject> enemyTracking = new List<GameObject>();
        List<GameObject> marking = new List<GameObject>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyTracking.Add(Instantiate(redCircle, Camera.main.WorldToScreenPoint(enemies[i].transform.position), Quaternion.identity, GameObject.Find("Canvas").transform));
            marking.Add(Instantiate(deathMark, Camera.main.WorldToScreenPoint(enemies[i].transform.position), Quaternion.identity, GameObject.Find("Canvas").transform));

        }

        while (true)
        {
            for (int i = 0; i < enemyTracking.Count; i++)
            {
                float temp = Vector3.Dot(Camera.main.transform.forward, enemyTracking[i].transform.position - transform.position);

                enemyTracking[i].transform.position = Camera.main.WorldToScreenPoint(enemies[i].transform.position);
                marking[i].transform.position = Camera.main.WorldToScreenPoint(enemies[i].transform.position);

            }

            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 temp = Camera.main.WorldToScreenPoint(enemies[i].transform.position);

                if (temp.x < 1920 && temp.x > 0 && temp.y < 1080 && temp.y > 0 && !targetInScreen.Contains(enemies[i].gameObject))
                {
                    
                        targetInScreen.Add(enemies[i].gameObject);
                    
                }
                else if (targetInScreen.Contains(enemies[i].gameObject))
                {

                    if (temp.x > 1920 || temp.x < 0 || temp.y > 1080 || temp.y < 0)
                    {
                        targetInScreen.Remove(enemies[i].gameObject);
                    }
                }
            }

            for (int i = 0; i < targetInScreen.Count; i++)
            {
                Vector3 dir = targetInScreen[i].transform.position - Camera.main.transform.position;
                Ray ray = new Ray(Camera.main.transform.position, dir);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 1000, ~playerMask))
                {
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
                    if (hitInfo.collider.gameObject.GetComponent<TestEnemy>())
                    {
                        hitInfo.collider.gameObject.GetComponent<TestEnemy>().ExposedTime += Time.deltaTime;
                    }
                }
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                float enemyTime = enemies[i].GetComponent<TestEnemy>().ExposedTime;
                Vector3 temp = enemyTracking[i].GetComponent<Image>().rectTransform.localScale;
                temp -= Vector3.one * enemyTime * 0.005f;
                temp.x = Mathf.Clamp(temp.x, 1f, 2.5f);
                temp.y = Mathf.Clamp(temp.y, 1f, 2.5f);
                temp.z = Mathf.Clamp(temp.z, 1f, 2.5f);
                enemyTracking[i].GetComponent<Image>().rectTransform.localScale = temp;
                if(temp.x < 1.1f)
                {
                    marking[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.7843f);
                }
            }


            if (!ishighnoon)
            {
                break;
            }

            yield return null;
        }
        

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<TestEnemy>().ExposedTime = 0;
            Destroy(enemyTracking[i]);
            Destroy(marking[i]);
        }
    }
    
    public List<GameObject> fireOrder = new List<GameObject>();
    IEnumerator HighnoonFire()
    {
        leftFire.isdelay = true;
        Dictionary<GameObject, float> tempDic = new Dictionary<GameObject, float>();
        for (int i = 0; i < targetInScreen.Count; i++)
        {
            tempDic.Add(targetInScreen[i], Camera.main.WorldToScreenPoint(targetInScreen[i].transform.position).x);
        }

        tempDic = tempDic.OrderByDescending(x => x.Value).ToDictionary(y => y.Key, y => y.Value);


        for (int i = 0; i < tempDic.Count; i++)
        {
            fireOrder.Add(tempDic.ElementAt(i).Key);
        }

        SoundManager.Instance.audioSources[3].Play();

        for (int i = 0; i < fireOrder.Count; i++)
        {

            Vector3 dir = fireOrder[i].transform.position - Camera.main.transform.position;
            Ray ray = new Ray(Camera.main.transform.position, dir);
            RaycastHit hitInfo;

            recoil.RecoilRotate(-40, 0, 0, 30);
            if (Physics.Raycast(ray, out hitInfo, 1000, ~playerMask))
            {
                GameObject bI1 = Instantiate(bulletImpact1);
                GameObject bI2 = Instantiate(bulletImpact2);
                GameObject bI3 = Instantiate(bulletImpact3);
                GameObject bI4 = Instantiate(bulletImpact4);
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

                CassidyMagazine.Instance.Magazine--;
                gunRecoilAnim.Stop();
                gunRecoilAnim.Play();

                yield return new WaitForSeconds(0.137f);
            }


            TestEnemy testEnemy = hitInfo.transform.GetComponent<TestEnemy>();
            if (testEnemy)
            {
                //피격 이벤트를 호출
                //enemy.OnDamageProcess(hitInfo.point);
                //GameObject bh = Instantiate(bodyHit, Camera.main.WorldToScreenPoint(Camera.main.transform.forward), Quaternion.identity, GameObject.Find("Canvas").transform);
                GameObject bh = Canvas.Instantiate(bodyHit, GameObject.Find("Canvas").transform);
                Destroy(bh, 0.1f);

            }
        }

        leftFire.isdelay = false;
        tempDic.Clear();
        fireOrder.Clear();
        SoundManager.Instance.audioSources[10].Play();
    }
}
