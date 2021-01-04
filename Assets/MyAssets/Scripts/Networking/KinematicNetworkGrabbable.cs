using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class KinematicNetworkGrabbable : MonoBehaviourPunCallbacks, IOnPhotonViewOwnerChange
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Rigidbody rigidbody;


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SetTheirsKinematicRPC();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        SetTheirsKinematicRPC();
    }

    public void SetTheirsKinematicRPC()
    {
        //Tell all clients to set kinematic if not theirs
        photonView.RPC("SetTheirsKinematic", RpcTarget.AllBuffered);
    }
    

    [PunRPC]
    private void SetTheirsKinematic()
    {
        if (!photonView.IsMine)
        {
            if (rigidbody != null) rigidbody.isKinematic = true;
        }
    }

    public void OnOwnerChange(Player newOwner, Player previousOwner)
    {
        SetTheirsKinematicRPC();
    }
}
