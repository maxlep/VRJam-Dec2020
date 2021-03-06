﻿using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

public class SceneReferenceManager : MonoBehaviour
{
    [SerializeField] [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    private List<TransformScenePair> TransformSceneList = new List<TransformScenePair>();
    
    [SerializeField] [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    private List<GameObjectScenePair> GameObjectSceneList = new List<GameObjectScenePair>();
    
    [SerializeField] [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    private List<ParticleSystemScenePair> ParticleSystemSceneList = new List<ParticleSystemScenePair>();
    
    [SerializeField] [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    private List<MMFeedbacksScenePair> MMFeedbacksSceneList = new List<MMFeedbacksScenePair>();
    
    [SerializeField] [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    private List<AnimatorScenePair> AnimatorSceneList = new List<AnimatorScenePair>();

    [SerializeField] [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    private List<CameraScenePair> CameraSceneList = new List<CameraScenePair>();
    
    [SerializeField] [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    private List<CharControllerScenePair> CharControllerSceneList = new List<CharControllerScenePair>();

    private void Awake()
    {
        foreach (var transformPair in TransformSceneList)
        {
            if (transformPair.AreValuesAssigned)
                transformPair.AssignValueToReference();
        }
        foreach (var gameObjectScenePair in GameObjectSceneList)
        {
            if (gameObjectScenePair.AreValuesAssigned)
                gameObjectScenePair.AssignValueToReference();
        }
        foreach (var particleSystemScenePair in ParticleSystemSceneList)
        {
            if (particleSystemScenePair.AreValuesAssigned)
                particleSystemScenePair.AssignValueToReference();
        }
        foreach (var mmFeedbacksScenePair in MMFeedbacksSceneList)
        {
            if (mmFeedbacksScenePair.AreValuesAssigned)
                mmFeedbacksScenePair.AssignValueToReference();
        }
        foreach (var animatorScenePair in AnimatorSceneList)
        {
            if (animatorScenePair.AreValuesAssigned)
                animatorScenePair.AssignValueToReference();
        }
        foreach (var cameraScenePair in CameraSceneList)
        {
            if (cameraScenePair.AreValuesAssigned)
                cameraScenePair.AssignValueToReference();
        }
        foreach (var charControllerScenePair in CharControllerSceneList)
        {
            if (charControllerScenePair.AreValuesAssigned)
                charControllerScenePair.AssignValueToReference();
        }
    }
}

[Serializable]
public class TransformScenePair
{
    [SerializeField] [LabelText("Reference")] [LabelWidth(125f)]
    [Required]
    private TransformSceneReference transformSceneReference;

    [SerializeField] [LabelText("Scene Value")] [LabelWidth(125f)]
    [Required] [SceneObjectsOnly]
    private Transform transformSceneValue;

    public bool AreValuesAssigned =>
        (transformSceneValue != null && transformSceneReference != null);
    
    public void AssignValueToReference()
    {
        transformSceneReference.Value = transformSceneValue;
    }
}

[Serializable]
public class GameObjectScenePair
{
    [SerializeField] [LabelText("Reference")] [LabelWidth(125f)]
    [Required]
    private TransformSceneReference gameObjectSceneReference;

    [SerializeField] [LabelText("Scene Value")] [LabelWidth(125f)]
    [Required] [SceneObjectsOnly]
    private Transform gameObjectSceneValue;

    public bool AreValuesAssigned =>
        (gameObjectSceneValue != null && gameObjectSceneReference != null);
    
    public void AssignValueToReference()
    {
        gameObjectSceneReference.Value = gameObjectSceneValue;
    }
}

[Serializable]
public class ParticleSystemScenePair
{
    [SerializeField] [LabelText("Reference")] [LabelWidth(125f)]
    [Required]
    private TransformSceneReference particleSystemSceneReference;

    [SerializeField] [LabelText("Scene Value")] [LabelWidth(125f)]
    [Required] [SceneObjectsOnly]
    private Transform particleSystemSceneValue;

    public bool AreValuesAssigned =>
        (particleSystemSceneValue != null && particleSystemSceneReference != null);
    
    public void AssignValueToReference()
    {
        particleSystemSceneReference.Value = particleSystemSceneValue;
    }
}

[Serializable]
public class MMFeedbacksScenePair
{
    [SerializeField] [LabelText("Reference")] [LabelWidth(125f)]
    [Required]
    private MMFeedbacksSceneReference feedbacksSceneReference;

    [SerializeField] [LabelText("Scene Value")] [LabelWidth(125f)]
    [Required] [SceneObjectsOnly]
    private MMFeedbacks feedbacksSceneValue;

    public bool AreValuesAssigned =>
        (feedbacksSceneValue != null && feedbacksSceneReference != null);
    
    public void AssignValueToReference()
    {
        feedbacksSceneReference.Value = feedbacksSceneValue;
    }
}

[Serializable]
public class AnimatorScenePair
{
    [SerializeField] [LabelText("Reference")] [LabelWidth(125f)]
    [Required]
    private AnimatorSceneReference animatorSceneReference;

    [SerializeField] [LabelText("Scene Value")] [LabelWidth(125f)]
    [Required] [SceneObjectsOnly]
    private Animator animatorSceneValue;

    public bool AreValuesAssigned =>
        (animatorSceneValue != null && animatorSceneReference != null);
    
    public void AssignValueToReference()
    {
        animatorSceneReference.Value = animatorSceneValue;
    }
}

[Serializable]
public class CameraScenePair
{
    [SerializeField] [LabelText("Reference")] [LabelWidth(125f)]
    [Required]
    private CameraSceneReference cameraSceneReference;

    [SerializeField] [LabelText("Scene Value")] [LabelWidth(125f)]
    [Required] [SceneObjectsOnly]
    private Camera cameraSceneValue;

    public bool AreValuesAssigned =>
        (cameraSceneValue != null && cameraSceneReference != null);
    
    public void AssignValueToReference()
    {
        cameraSceneReference.Value = cameraSceneValue;
    }
}

[Serializable]
public class CharControllerScenePair
{
    [SerializeField] [LabelText("Reference")] [LabelWidth(125f)]
    [Required]
    private CharControllerSceneReference charControllerSceneReference;

    [SerializeField] [LabelText("Scene Value")] [LabelWidth(125f)]
    [Required] [SceneObjectsOnly]
    private CharacterController charControllerSceneValue;

    public bool AreValuesAssigned =>
        (charControllerSceneValue != null && charControllerSceneReference != null);
    
    public void AssignValueToReference()
    {
        charControllerSceneReference.Value = charControllerSceneValue;
    }
}
