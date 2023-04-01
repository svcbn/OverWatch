using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassidyRecoil : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;
    Vector3 currentPosition;
    Vector3 targetPosition;

    [SerializeField] float snappiness;
    [SerializeField] float RS;
    [SerializeField] float RS2;

    // Update is called once per frame
    public void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, RS * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, RS2 * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);
        transform.localPosition = currentPosition;
    }

    public void RecoilRotate(float recoilX, float recoilY, float recoilZ, float returnSpeed)
    {
        RS = returnSpeed;
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
    
    public void RecoilRotate(float recoilX, float recoilY, float recoilZ, float returnSpeed, int certain)
    {
        RS = returnSpeed;
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), recoilZ);
    }

    public void RecoilMove(float recoilX, float recoilY, float recoilZ, float returnSpeed)
    {
        RS2 = returnSpeed;
        targetPosition += new Vector3(recoilX, recoilY, recoilZ);
    }
}
