using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update

    public static CameraShake Instance;
    GameObject cam;
    Vector3 cameraOriginalPos;

    private void Awake()
    {
        Instance = this;
        cam = GameObject.Find("CameraRecoil");
        cameraOriginalPos = cam.transform.position;
    }


    public IEnumerator Shake(float duration = 1f, float magnitude = 1f)
    {
        float timer = 0;

        while (timer <= duration)
        {
            cam.transform.localPosition = Random.insideUnitSphere * magnitude + cameraOriginalPos;

            timer += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = cameraOriginalPos;
    }

}
