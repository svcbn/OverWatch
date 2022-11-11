using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotSpeed = 205;

    float mx;
    float my;

    // Start is called before the first frame update
    void Start()
    {
        mx = transform.eulerAngles.y;
        my = -transform.eulerAngles.x;
        transform.eulerAngles = new Vector3(-my, mx, 0);
        Cursor.lockState = CursorLockMode.Locked;
        

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;

        // ½Ã¾ß°¢
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
