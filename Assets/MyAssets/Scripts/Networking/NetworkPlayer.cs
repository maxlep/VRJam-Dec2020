﻿using System;
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
    [SerializeField] private CharacterController charController;
    [SerializeField] private CharControllerSceneReference charControllerRig;

    // Start is called before the first frame update
    void Start()
    {
        //Disable renderers for my networked player
        if (photonView.IsMine)
        {
            foreach (var rend in gameObject.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
            
            //Ignore collisions between rig and your own networked player
            Collider charControllerCollider = charController.GetComponent<Collider>();
            Collider charControllerRigCollider = charControllerRig.Value.GetComponent<Collider>();
            Physics.IgnoreCollision(charControllerCollider, charControllerRigCollider);
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
            MapCharController(charController, charControllerRig.Value);
        }
    }

    private void MapTransform(Transform source, Transform target)
    {
        source.position = target.position;
        source.rotation = target.rotation;
    }

    private void MapCharController(CharacterController source, CharacterController target)
    {
        source.transform.position = target.transform.position;
        source.center = target.center;
        source.height = target.height;
        source.radius = target.radius;
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
