using System.Collections;
using System.Collections.Generic;
using BNG;
using MyAssets.Scripts.Interactions;
using Photon.Pun;
using UnityEngine;

public class ShrinkGun : MonoBehaviour
{
    [SerializeField] private GrabbedControllerBinding fireBinding;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TriggerProxy triggerProxy;
    [SerializeField] private float scaleRate = 1f;

    private Grabber currentGrabber;
    
    
    private void Update()
    {
        if (currentGrabber == null) return;
        

        if (InputBridge.Instance.GetGrabbedControllerBinding(fireBinding, currentGrabber.HandSide))
        {
            if (photonView.IsMine)
            {
                Fire();
            }
        }
    }
    
    public void Fire()
    {
        foreach (var networkPlayerObj in triggerProxy.ObjectsInRange)
        {
            NetworkPlayer networkPlayerScript = networkPlayerObj.transform.parent.GetComponent<NetworkPlayer>();
            networkPlayerScript.ScalePlayerUp(scaleRate);
        }
    }
    
    public void SetGrabber(Grabber value)
    {
        currentGrabber = value;
    }

    public void UnsetGrabber()
    {
        currentGrabber = null;
    }
}
