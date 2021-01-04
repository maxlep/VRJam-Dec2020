using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using EnhancedHierarchy;
using Photon.Pun;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Grabbable grabbableScript;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Light light;
    [SerializeField] private float batteryDuration = 10f;
    [SerializeField] private int shakesToCharge = 10;
    [SerializeField] private float minShakeSpeed = 5f;
    [SerializeField] private float minShakeAngle = 100f;
    [SerializeField] private float chargeVibrateFrequency = 1f;
    [SerializeField] private float chargeVibrateAmplitude = 1f;
    [SerializeField] private float chargeVibrateDuration = .5f;
    [SerializeField] private float flickerDuration = .5f;
    [SerializeField] private AnimationCurve flickerCurve;

    private Vector3 currentVelocity;
    private Vector3 previousVelocity;
    private float lastChargeTime;
    private float startIntensity;
    private int currentShakes;
    private bool isFlickering;
    private bool isOn = true;

    private void Awake()
    {
        startIntensity = light.intensity;
    }

    private void Update()
    {
        CheckForTurnOff();
        CheckForShake();
    }
    
    private void CheckForTurnOff()
    {
        if (!isOn) return;
        
        if (!isFlickering && lastChargeTime + batteryDuration < Time.time)
        {
            isFlickering = true;
            LTDescr flickerTween = LeanTween.value(0f, 1f, flickerDuration);
            flickerTween.setOnUpdate(t =>
            {
                float flickerValue = Mathf.Clamp01(flickerCurve.Evaluate(t));
                light.intensity = flickerValue * startIntensity;
            });
            flickerTween.setOnComplete(_ =>
            {
                light.intensity = startIntensity;
                isFlickering = false;
                TurnOff();
            });
        }
    }

    private void TurnOff()
    {
        isOn = false;
        light.enabled = false;
    }

    [PunRPC]
    private void TurnOn()
    {
        isOn = true;
        light.enabled = true;
        lastChargeTime = Time.time;
        currentShakes = 0;
    }


    private void CheckForShake()
    {
        if (isOn) return;
        
        Grabber currentGrabber = grabbableScript.GetPrimaryGrabber();
        if (currentGrabber == null)
            return;
        
        currentVelocity = currentGrabber.velocityTracker.GetVelocity();
        float deltaVelocityAngle = Vector3.Angle(previousVelocity.normalized, currentVelocity.normalized);
        float currentSpeed = currentVelocity.magnitude;

        if (currentSpeed > minShakeSpeed && deltaVelocityAngle > minShakeAngle)
        {
            RegisterShake();
        }

        previousVelocity = currentVelocity;
    }

    private void RegisterShake()
    {
        currentShakes++;
        InputBridge.Instance.VibrateController(chargeVibrateFrequency, chargeVibrateAmplitude, 
            chargeVibrateDuration, grabbableScript.GetPrimaryGrabber().HandSide);
        
        if (currentShakes >= shakesToCharge)
        {
            photonView.RPC("TurnOn", RpcTarget.AllBuffered);
            //TurnOn();
        }
    }

   
}
