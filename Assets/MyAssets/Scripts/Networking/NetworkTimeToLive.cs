using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkTimeToLive : MonoBehaviour
{
    [SerializeField] private float secondsToLive;
    [SerializeField] private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.value(0f, 1f, secondsToLive).setOnComplete(() =>
        {
            PhotonNetwork.Destroy(photonView);
        });
    }
}
