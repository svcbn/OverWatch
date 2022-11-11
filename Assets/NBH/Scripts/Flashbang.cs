using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float power = 5f;
    float currentTime = 0;

    GameObject flashBang;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce((Camera.main.transform.forward + Camera.main.transform.up * 0.2f).normalized * power);
        flashBang = (GameObject)Resources.Load("FlashBang");
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > 0.2f)
        {
            GameObject fb = Instantiate(flashBang);
            fb.transform.position = transform.position;

            Destroy(gameObject);
            SoundManager.Instance.audioSources[7].Play();
            Destroy(fb, 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        GameObject fb = Instantiate(flashBang);
        fb.transform.position = transform.position;

        Destroy(gameObject);
        SoundManager.Instance.audioSources[7].Play();
        Destroy(fb, 2f);
    }
}
