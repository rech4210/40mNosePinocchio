using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoSingleton<CameraController>
{
    private float shakePower = 0.0f;
    private float shakeAmount = 7.5f;

    private bool isXShake = true;
    private bool isYShake = true;

    private float angleAmount = 0.0f;
    private float scaleAmount = 1.0f;
    
    public Vector3 Offset = Vector3.zero;
    private Vector3 anchorPoint = Vector3.zero;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }
    
    private void LateUpdate()
    {
        shakePower -= shakePower / shakeAmount;
        
        Vector3 shakeVec = Vector3.zero;
        shakeVec.x = (isXShake) ? Random.Range(-shakePower, shakePower) : 0.0f;
        shakeVec.y = (isYShake) ? Random.Range(-shakePower, shakePower) : 0.0f;
        transform.position = anchorPoint + Offset + shakeVec;
        
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0.0f, 0.0f, angleAmount), 5.0f * Time.deltaTime);
    }
    
    public void OnShake(float power, float amount, bool xshake = true, bool yshake = true)
    {
        shakePower = power;
        shakeAmount = amount;
        isXShake = xshake;
        isYShake = yshake;
    }
}