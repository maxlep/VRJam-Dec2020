using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ParticleHelper : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    public void DisableEmissions(float delay)
    {
        LeanTween.value(0f, 1f, delay)
            .setOnComplete(DisableEmissions);
    }

    public void DisableEmissions()
    {
        foreach (var partSystem in particleSystems)
        {
            partSystem.Stop();
        }
    }

    [Button(ButtonSizes.Large)]
    public void PopulateParticleSystems()
    {
        particleSystems.Clear();
        particleSystems = GetComponentsInChildren<ParticleSystem>().ToList();
    }
}
