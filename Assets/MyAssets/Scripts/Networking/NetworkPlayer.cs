using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using MyAssets.Scripts.Interactions;
using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

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
    [SerializeField] private TransformSceneReference playerControllerSceneReference;
    [SerializeField] private TransformSceneReference holsterLeftSceneReference;
    [SerializeField] private TransformSceneReference holsterRightSceneReference;
    [SerializeField] private TransformSceneReference shoulderLeftSceneReference;
    [SerializeField] private TransformSceneReference shoulderRightSceneReference;
    [SerializeField] private string holsterLeftGrabbablePath;
    [SerializeField] private string holsterRightGrabbablePath;
    [SerializeField] private string shoulderLeftGrabbablePath;
    [SerializeField] private string shoulderRightGrabbablePath;
    [SerializeField] private ResizePlayer resizeScript;
    
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

            InstantiateEquipGrabbables();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            MapLocRot(head, headTargetRig.Value);
            MapLocRot(leftHand, leftHandTargetRig.Value);
            MapLocRot(rightHand, rightHandTargetRig.Value);
            UpdateHandAnimation(leftHandAnimator, leftHandAnimatorRig.Value);
            UpdateHandAnimation(rightHandAnimator, rightHandAnimatorRig.Value);
            MapCharController(charController, charControllerRig.Value);
            
            //Map scale from networked player -> local rig (shrink gun)
            MapScale(playerControllerSceneReference.Value, head);
        }
    }

    private void MapLocRot(Transform source, Transform target)
    {
        source.position = target.position;
        source.rotation = target.rotation;
    }
    
    private void MapScale(Transform source, Transform target)
    {
        source.localScale = target.localScale;
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

    public void ScalePlayerUp(float rate)
    {
        //if (photonView.IsMine)
        
        photonView.RPC("ScaleUp", RpcTarget.AllBuffered, rate);
        
    }

    [PunRPC]
    public void ScaleUp(float rate)
    {
        resizeScript.ScaleUp(rate);
    }

    private void InstantiateEquipGrabbables()
    {
        if (!holsterLeftGrabbablePath.IsNullOrEmpty())
        {
            GameObject leftHolsterObj = PhotonNetwork.Instantiate(holsterLeftGrabbablePath, 
                transform.position, transform.rotation);
            Grabbable grabbableScript = leftHolsterObj.GetComponent<Grabbable>();
            SnapZone snapZone = holsterLeftSceneReference.Value.GetComponent<SnapZone>();
            snapZone.GrabGrabbable(grabbableScript);
        }
    }
}
