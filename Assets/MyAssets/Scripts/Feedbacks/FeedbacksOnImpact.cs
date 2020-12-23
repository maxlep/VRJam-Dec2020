using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbacksOnImpact : MonoBehaviour
{
    [SerializeField] private MMFeedbacks feedbacks;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        feedbacks.PlayFeedbacks();
    }

    private void OnTriggerEnter(Collider other)
    {
        feedbacks.PlayFeedbacks();
    }
}
