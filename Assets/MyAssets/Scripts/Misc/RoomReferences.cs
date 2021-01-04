using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using MyAssets.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoomReferences : MonoBehaviour
{
    [SerializeField] private Transform frontWall;
    [SerializeField] private Transform rightWall;
    [SerializeField] private Transform backWall;
    [SerializeField] private Transform leftWall;
    
    [SerializeField] [ReadOnly] private GameObject frontWallGlobal;
    [SerializeField] [ReadOnly] private GameObject rightWallGlobal;
    [SerializeField] [ReadOnly] private GameObject backWallGlobal;
    [SerializeField] [ReadOnly] private GameObject leftWallGlobal;

    public GameObject FrontWallGlobal => frontWallGlobal;
    public GameObject RightWallGlobal => rightWallGlobal;
    public GameObject BackWallGlobal => backWallGlobal;
    public GameObject LeftWallGlobal => leftWallGlobal;
    

    //Sort walls in list from north -> West (clockwise) and account for rotation from global
    public void GlobalAlignWalls()
    {
        Vector3 frontWallDirection = (frontWall.position - transform.position).xoz().normalized;

        float angleFromGlobalForward = Vector3.Angle(frontWallDirection, Vector3.forward);
        float angleFromGlobalRight = Vector3.Angle(frontWallDirection, Vector3.right);
        float angleFromGlobalBack = Vector3.Angle(frontWallDirection, Vector3.back);
        float angleFromGlobalLeft = Vector3.Angle(frontWallDirection, Vector3.left);

        float[] angleArray = {angleFromGlobalForward, angleFromGlobalRight, angleFromGlobalBack, angleFromGlobalLeft};
        float minAngle = Mathf.Min(angleArray);

        if (Mathf.Approximately(angleFromGlobalForward, minAngle))
        {
            frontWallGlobal = frontWall.gameObject;
            rightWallGlobal = rightWall.gameObject;
            backWallGlobal = backWall.gameObject;
            leftWallGlobal = leftWall.gameObject;
        }
        else if (Mathf.Approximately(angleFromGlobalRight, minAngle))
        {
            frontWallGlobal = leftWall.gameObject;
            rightWallGlobal = frontWall.gameObject;
            backWallGlobal = rightWall.gameObject;
            leftWallGlobal = backWall.gameObject;
        }
        else if (Mathf.Approximately(angleFromGlobalBack, minAngle))
        {
            frontWallGlobal = backWall.gameObject;
            rightWallGlobal = leftWall.gameObject;
            backWallGlobal = frontWall.gameObject;
            leftWallGlobal = rightWall.gameObject;
        }
        else
        {
            frontWallGlobal = rightWall.gameObject;
            rightWallGlobal = backWall.gameObject;
            backWallGlobal = leftWall.gameObject;
            leftWallGlobal = frontWall.gameObject;
        }
    }
}
