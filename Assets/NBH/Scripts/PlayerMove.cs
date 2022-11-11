using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 4;
    public float gravity = -20f;
    public float jumpPower = 5f;

    float yVelocity = 0;
    bool isJumping;

    CharacterController cc;

    CassidyRecoil recoil;
    float soundDelay = 0.85f;
    float walkDelay = 0.4f;
    float currentTime = 0;
    float walkTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        recoil = GetComponentInChildren<CassidyRecoil>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        walkTime += Time.deltaTime;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);

        if(dir != Vector3.zero && currentTime > soundDelay)
        {

            SoundManager.Instance.audioSources[4].Play();

            currentTime = 0;
        }

        if (dir != Vector3.zero && walkTime > walkDelay)
        {
            recoil.RecoilMove(0, -10f, 0, 100);

            walkTime = 0;
        }


        yVelocity += gravity * Time.deltaTime;

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;

        }

        dir.y = yVelocity;

        cc.Move(dir * speed * Time.deltaTime);

    }

}
