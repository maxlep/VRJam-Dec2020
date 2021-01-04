using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using MyAssets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private float strideLength = .25f;
    [SerializeField] private List<AudioClip> footStepSounds = new List<AudioClip>();

    private float distanceTraveled;
    private Vector3 lastPosition;

    private void Update()
    {
        float deltaDistance = (transform.position - lastPosition).xoz().magnitude;
        distanceTraveled += deltaDistance;

        if (distanceTraveled >= strideLength)
        {
            distanceTraveled -= strideLength;
            EffectsManager.Instance.PlayClipAtPoint(GetRandomClip(footStepSounds), transform.position);
        }

        
        
        
        lastPosition = transform.position;
    }

    private AudioClip GetRandomClip(List<AudioClip> audioClipList)
    {
        int randomIndex = Random.Range(0, audioClipList.Count);
        return audioClipList[randomIndex];
    }
}
