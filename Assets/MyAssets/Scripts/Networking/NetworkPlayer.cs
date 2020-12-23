using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
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

    [SerializeField] private AnimatorSceneReference leftHandAnimatorRig;
    [SerializeField] private AnimatorSceneReference rightHandAnimatorRig;
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator rightHandAnimator;

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
            UpdateHandAnimation(leftHandAnimator, leftHandAnimatorRig.Value);
            UpdateHandAnimation(rightHandAnimator, rightHandAnimatorRig.Value);
        }
    }

    private void MapTransform(Transform source, Transform target)
    {
        source.position = target.position;
        source.rotation = target.rotation;
    }

    private void UpdateHandAnimation(Animator animator, Animator target)
    {
        Action<string> copyParam = (string paramName) =>
        {
            animator.SetFloat(paramName, target.GetFloat(paramName));
        };
        
        Action<int> copyLayerWeight = (int layerIndex) =>
        {
            animator.SetLayerWeight(layerIndex, target.GetLayerWeight(layerIndex));
        };

        copyParam("Pinch");
        copyParam("Flex");
        animator.SetInteger("Pose", target.GetInteger("Pose"));

        copyLayerWeight(0);
        copyLayerWeight(1);
        copyLayerWeight(2);
    }
}
