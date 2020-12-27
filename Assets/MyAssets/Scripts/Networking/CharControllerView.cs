using System;
using Photon.Pun;
using UnityEngine;

namespace MyAssets.Scripts.Networking
{
    public class CharControllerView : MonoBehaviourPun, IPunObservable
    {
        private CharacterController charController;
        public void Awake()
        {
            charController = GetComponent<CharacterController>();
        }
        
        
         public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            var tr = transform;

            // Write
            if (stream.IsWriting)
            {
                stream.SendNext(charController.center);
                stream.SendNext(charController.height);
                stream.SendNext(charController.radius);
            }
            // Read
            else
            {
                charController.center = (Vector3)stream.ReceiveNext();
                charController.height = (float)stream.ReceiveNext();
                charController.radius = (float)stream.ReceiveNext();
            }
        }
    }
}