using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CassidySkillUI : MonoBehaviour
{
    #region StunGranade
    public Image eIcon;
    public Image eReverseAlpha;
    public Image eCoolDownBG;
    public Image eOrange;
    public Image eCoolDownBorder;
    public Image eForbidBorder;
    public Image eForbidBG;
    public Text eCoolDownText;
    public Image eKey;
    public ParticleSystem eCoolDownFX;

    float _eCurTime = 10;
    public bool isECooldown = false;
    #endregion

    #region Roll
    public Image sIcon;
    public Image sReverseAlpha;
    public Image sCoolDownBG;
    public Image sOrange;
    public Image sCoolDownBorder;
    public Text sCoolDownText;
    public Image sForbidBorder;
    public Image sForbidBG;
    public Image sKey;
    public ParticleSystem sCoolDownFX;

    float _sCurTime = 6;
    public static bool isSCooldown = false;
    public bool IsSCoolDown
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
    #endregion

    public static CassidySkillUI Instance;

    float ECurTime
    {
        get
        {
            return _eCurTime;
        }
        set
        {
            _eCurTime = value;
        }
    }

    float SCurTime
    {
        get
        {
            return _sCurTime;
        }
        set
        {
            _sCurTime = value;
        }
    }


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region StunGranade Start
        eIcon.color = new Color(1, 1, 1, 0.7843f);
        eReverseAlpha.color = new Color(1, 1, 1, 0);
        eCoolDownBG.color = new Color(1, 1, 1, 0);
        eCoolDownText.color = new Color(1, 1, 1, 0);
        ECurTime = 10f;
        eCoolDownText.transform.localScale = Vector3.one * 0.1f;
        eCoolDownBorder.color = new Color(1, 1, 1, 0);
        eForbidBorder.color = new Color(1, 1, 1, 0);
        eForbidBG.color = new Color(0, 0, 0, 0);
        eKey.color = new Color(1, 1, 1, 0.7843f);
        eOrange.rectTransform.localPosition = new Vector3(-23.4f, -130f);
        #endregion


        sIcon.color = new Color(1, 1, 1, 0.7843f);
        sReverseAlpha.color = new Color(1, 1, 1, 0);
        sCoolDownBG.color = new Color(1, 1, 1, 0);
        sCoolDownText.color = new Color(1, 1, 1, 0);
        SCurTime = 6f;
        sCoolDownText.transform.localScale = Vector3.one * 0.1f;
        sCoolDownBorder.color = new Color(1, 1, 1, 0);
        sForbidBorder.color = new Color(1, 1, 1, 0);
        sForbidBG.color = new Color(0, 0, 0, 0);
        sKey.color = new Color(1, 1, 1, 0.7843f);
        sOrange.rectTransform.localPosition = new Vector3(-23.4f, -130f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isECooldown && !CassidyRoll.Isroll && !CassidyHighnoon.ishighnoon)
        {
            isECooldown = true;
            StartCoroutine(eCoolDown());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSCooldown && !CassidyHighnoon.ishighnoon)
        {
            isSCooldown = true;
            StartCoroutine(sCoolDown());
        }

        if(CassidyHighnoon.ishighnoon)
        {
            sForbidBorder.color = new Color(1, 1, 1, 1);
            sForbidBG.color = new Color(0, 0, 0, 0.7058f);
            eForbidBorder.color = new Color(1, 1, 1, 1);
            eForbidBG.color = new Color(0, 0, 0, 0.7058f);
        }
        else
        {
            sForbidBorder.color = new Color(1, 1, 1, 0);
            sForbidBG.color = new Color(0, 0, 0, 0);
            eForbidBorder.color = new Color(1, 1, 1, 0);
            eForbidBG.color = new Color(0, 0, 0, 0);
        }

        eCoolDownText.text = Mathf.Ceil(ECurTime) + "";
        sCoolDownText.text = Mathf.Ceil(SCurTime) + "";
    }

    IEnumerator eCoolDown()
    {
        float currentTime = 0;

        eIcon.color = new Color(0.8941f, 0.5125f, 0.1960f, 0.7843f);
        Color eK = eKey.color;

        eCoolDownText.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.2f);
        eIcon.color = new Color(0.8941f, 0.5125f, 0.1960f, 0);
        eIcon.color = new Color(1, 1, 1, 0);
        eCoolDownBG.color = new Color(1, 1, 1, 1);
        Color eCDBG = eCoolDownBG.color;

        eCoolDownBorder.color = new Color(1f, 1f, 1f, 1f);
        eReverseAlpha.color = new Color(1, 1, 1, 0.7843f);

        while (true)
        {
            currentTime += Time.deltaTime;
            ECurTime -= Time.deltaTime;

            eCoolDownBG.color = eCDBG;
            eCDBG.a = Mathf.Lerp(eCDBG.a, 0.004f, 5 * Time.deltaTime);
            
            eKey.color = eK;
            eK.a = Mathf.Lerp(eK.a, 0.1176f, 10 * Time.deltaTime);

            eCoolDownText.transform.localScale = Vector3.Lerp(eCoolDownText.transform.localScale, Vector3.one, 10 * Time.deltaTime);

            if (currentTime > 1f)
            {
                break;
            }
            yield return null;
        }

        while(true)
        {
            currentTime += Time.deltaTime;
            ECurTime -= Time.deltaTime;
            eOrange.rectTransform.localPosition += Vector3.up * 10 * Time.deltaTime;

            if(currentTime > 10f)
            {
                break;
            }
            yield return null;
        }

        eCoolDownFX.Play();
        eIcon.color = new Color(1, 1, 1, 0.7843f);
        eReverseAlpha.color = new Color(1, 1, 1, 0);
        eCoolDownBG.color = new Color(1, 1, 1, 0);
        eCoolDownText.color = new Color(1, 1, 1, 0);
        ECurTime = 10f;
        eCoolDownText.transform.localScale = Vector3.one * 0.1f;
        eCoolDownBorder.color = new Color(1, 1, 1, 0);
        eKey.color = new Color(1, 1, 1, 0.7843f);
        eOrange.rectTransform.localPosition = new Vector3(-23.4f, -130f);

        isECooldown = false;
        CassidyStunGranade.IsECooldown = false;
        SoundManager.Instance.audioSources[8].Play();

    }

    IEnumerator sCoolDown()
    {
        float currentTime = 0;

        sIcon.color = new Color(0.8941f, 0.5125f, 0.1960f, 0.7843f);
        Color sK = sKey.color;

        sCoolDownText.color = new Color(1, 1, 1, 1);

        eForbidBorder.color = new Color(1, 1, 1, 1);
        eForbidBG.color = new Color(0, 0, 0, 0.7058f);

        yield return new WaitForSeconds(0.2f);

        sIcon.color = new Color(0.8941f, 0.5125f, 0.1960f, 0);
        sIcon.color = new Color(1, 1, 1, 0);
        sCoolDownBG.color = new Color(1, 1, 1, 1);
        Color sCDBG = sCoolDownBG.color;

        sCoolDownBorder.color = new Color(1f, 1f, 1f, 1f);
        sReverseAlpha.color = new Color(1, 1, 1, 0.7843f);

        eForbidBG.color = new Color(0, 0, 0, 0);
        eForbidBorder.color = new Color(1, 1, 1, 0);


        while (true)
        {
            currentTime += Time.deltaTime;
            SCurTime -= Time.deltaTime;

            sCoolDownBG.color = sCDBG;
            sCDBG.a = Mathf.Lerp(sCDBG.a, 0.004f, 5 * Time.deltaTime);

            sKey.color = sK;
            sK.a = Mathf.Lerp(sK.a, 0.1176f, 10 * Time.deltaTime);

            sCoolDownText.transform.localScale = Vector3.Lerp(sCoolDownText.transform.localScale, Vector3.one, 10 * Time.deltaTime);

            if (currentTime > 1f)
            {
                break;
            }
            yield return null;
        }

        while (true)
        {
            currentTime += Time.deltaTime;
            SCurTime -= Time.deltaTime;
            sOrange.rectTransform.localPosition += Vector3.up * 17f * Time.deltaTime;

            if (currentTime > 6f)
            {
                break;
            }
            yield return null;
        }

        sCoolDownFX.Play();
        sIcon.color = new Color(1, 1, 1, 0.7843f);
        sReverseAlpha.color = new Color(1, 1, 1, 0);
        sCoolDownBG.color = new Color(1, 1, 1, 0);
        sCoolDownText.color = new Color(1, 1, 1, 0);
        SCurTime = 6f;
        sCoolDownText.transform.localScale = Vector3.one * 0.1f;
        sCoolDownBorder.color = new Color(1, 1, 1, 0);
        sKey.color = new Color(1, 1, 1, 0.7843f);
        sOrange.rectTransform.localPosition = new Vector3(-23.4f, -130f);


        isSCooldown = false;
        CassidyRoll.IsSCoolDown = false;
        SoundManager.Instance.audioSources[8].Play();
    }
}
