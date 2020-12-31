using System;
using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;
using UnityEngine;

public class RaycastProjectile : MonoBehaviour
{
    public delegate void OnDestroyed();

    public event OnDestroyed onDestroyed;
    
    private float speed;
    private bool firstUpdate = true;

    public void Init(float speed)
    {
        this.speed = speed;
    }

    private void OnDestroy()
    {
        if (onDestroyed != null) onDestroyed.Invoke();
    }

    private void Update()
    {
        //Dont move particles immediately after spawn
        if (firstUpdate)
        {
            firstUpdate = false;
            return;
        }
        
        //Move particle
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
