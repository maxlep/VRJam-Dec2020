using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CharControllerSceneReference", menuName = "SceneReferences/CharControllerSceneReference", order = 0)]
public class CharControllerSceneReference : SceneReference
{
    [SerializeField] private CharacterController charControllerReference;
    
    public CharacterController Value
    {
        get => charControllerReference;
        set => charControllerReference = value;
    }
}