using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NetworkTooltip : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Text tooltipText;
    
    

    // Update is called once per frame
    void Update()
    {
        tooltipText.text = GetTooltipLabel();
    }

    private string GetTooltipLabel()
    {
        var owner = photonView.Owner;
        
        if (owner == null)
            return "";

        string ownerNickname = owner.NickName;

        if (ownerNickname == null)
            return "";

        return ownerNickname; 
    }
}
