﻿using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "MMFeedbacksSceneReference", menuName = "SceneReferences/MMFeedbacksSceneReference", order = 0)]
public class MMFeedbacksSceneReference : SceneReference
{
    [SerializeField] private MMFeedbacks feedbacksReference;
    
    public MMFeedbacks Value
    {
        get => feedbacksReference;
        set => feedbacksReference = value;
    }
}