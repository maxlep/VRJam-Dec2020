using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

public class ToggleNetworkedComponents : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<MonoBehaviour> componentsToEnable = new List<MonoBehaviour>();

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        EnableTheirComponents(false);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        EnableTheirComponents(true);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        
        //If i became master client, enable scripts again, otherwise disable
        if (photonView.Controller.IsMasterClient)
            EnableMyComponents(true);
        else
            EnableMyComponents(false);
    }

    private void EnableTheirComponents(bool enabled)
    {
        if (!photonView.IsMine)
        {
            foreach (var component in componentsToEnable)
            {
                component.enabled = enabled;
            }

            if (agent != null) agent.enabled = enabled;
        }
    }
    
    private void EnableMyComponents(bool enabled)
    {
        if (photonView.IsMine)
        {
            foreach (var component in componentsToEnable)
            {
                component.enabled = enabled;
            }

            if (agent != null) agent.enabled = enabled;
        }
    }
}
