using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverRotate : MonoBehaviour
{
    public float rotateSpeed = 5f;

    float mx;
    float my;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mx += h * rotateSpeed * Time.deltaTime;
        my += v * rotateSpeed * Time.deltaTime;

        mx = Mathf.Clamp(mx, -1f, 1f);
        my = Mathf.Clamp(my, -1f, 1f);
        transform.localEulerAngles = new Vector3(-my, mx, 0);

    }
}
