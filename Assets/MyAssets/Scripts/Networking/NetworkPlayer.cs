using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private TransformSceneReference headTargetRig;
    [SerializeField] private TransformSceneReference leftHandTargetRig;
    [SerializeField] private TransformSceneReference rightHandTargetRig;

    // Start is called before the first frame update
    void Start()
    {
        //Disable renderers for my networked hands.
        if (photonView.IsMine)
        {
            foreach (var rend in head.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
            foreach (var rend in leftHand.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
            foreach (var rend in rightHand.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            MapTransform(head, headTargetRig.Value);
            MapTransform(leftHand, leftHandTargetRig.Value);
            MapTransform(rightHand, rightHandTargetRig.Value);
        }
    }

    private void MapTransform(Transform source, Transform target)
    {
        source.position = target.position;
        source.rotation = target.rotation;
    }
}
