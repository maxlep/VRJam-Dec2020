using Photon.Pun;
using UnityEngine;

namespace MyAssets.Scripts.Interactions
{
    public class SimpleShoot : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float shootForce;
        [SerializeField] private Transform firePoint;

        public void DoShoot()
        {
            var instantiated = PhotonNetwork.Instantiate("Object/Network Cube", firePoint.position, firePoint.rotation);
            var rb = instantiated.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * shootForce;
        }
    }
}