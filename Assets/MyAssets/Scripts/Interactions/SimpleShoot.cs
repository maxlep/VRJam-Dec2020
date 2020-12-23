using System;
using System.Net.Http.Headers;
using BNG;
using Photon.Pun;
using UnityEngine;

namespace MyAssets.Scripts.Interactions
{
    public class SimpleShoot : MonoBehaviour
    {
        [SerializeField] private GrabbedControllerBinding fireModeBinding;
        [SerializeField] private GrabbedControllerBinding fireBinding;
        [SerializeField] private GrabbedControllerBinding fireHoldBinding;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GameObject prefab;
        [SerializeField] private float shootForce;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float cooldown = 0.2f;
        private bool canShoot = true, fireHeld;
        private bool automaticFire = true;

        private Grabber currentGrabber;

        private void Update()
        {
            if (currentGrabber == null) return; 
            
            if (InputBridge.Instance.GetGrabbedControllerBinding(fireModeBinding, currentGrabber.HandSide))
            {
                ToggleFireMode();
            }

            if (InputBridge.Instance.GetGrabbedControllerBinding(fireBinding, currentGrabber.HandSide))
            {
                if (canShoot)
                {
                    canShoot = false;
                    
                    // Fire();
                    photonView.RPC("Fire", RpcTarget.AllBuffered);

                    LeanTween.value(0f, 1f, cooldown).setOnComplete(() =>
                    {
                        canShoot = true;
                    });
                }
            } else if (InputBridge.Instance.GetGrabbedControllerBinding(fireHoldBinding, currentGrabber.HandSide))
            {
                if (canShoot && automaticFire)
                {
                    canShoot = false;

                    // Fire();
                    photonView.RPC("Fire", RpcTarget.AllBuffered);

                    LeanTween.value(0f, 1f, cooldown).setOnComplete(() =>
                    {
                        canShoot = true;
                    });
                }
            }
        }

        [PunRPC]
        public void Fire()
        {
            var instantiated = PhotonNetwork.Instantiate("Object/Network Cube", firePoint.position, firePoint.rotation);
            var rb = instantiated.GetComponent<Rigidbody>();
            rb.velocity = -firePoint.right * shootForce;
        }

        private void ToggleFireMode()
        {
            if (photonView.IsMine)
            {
                automaticFire = !automaticFire;
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
}