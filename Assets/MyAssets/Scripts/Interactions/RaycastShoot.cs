using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask fireMask;
    [SerializeField] private float shootParticleSpeed = 100f;
    [SerializeField] private GameObject shootParticlesPrefab;
    [SerializeField] private List<GameObject> impactParticlesPrefabs;

    public void Fire()
    {
        //TODO: Spawn shoot particles, play shoot sound
        
        
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit,
            Mathf.Infinity, fireMask))
        {
            //TODO: Check layer and deal damage
            
            //Spawn particles with speed lifetime until impact then spawn impact particle on destroy
            float impactTime = hit.distance / shootParticleSpeed;
            GameObject shootParticles = EffectsManager.Instance.SpawnParticlesAtPoint(shootParticlesPrefab,
                firePoint.position, firePoint.rotation, true, impactTime);
            RaycastProjectile projectileScript = shootParticles.GetComponent<RaycastProjectile>();
            projectileScript.Init(shootParticleSpeed);
            projectileScript.onDestroyed += () => SpawnImpactParticles(hit.point, hit.normal);
        }
        else
        {
            //Spawn particles with speed default lifetime
            GameObject shootParticles = EffectsManager.Instance.SpawnParticlesAtPoint(shootParticlesPrefab,
                firePoint.position, firePoint.rotation);
            shootParticles.GetComponent<RaycastProjectile>().Init(shootParticleSpeed);
        }
    }

    private void SpawnImpactParticles(Vector3 position, Vector3 normal)
    {
        //Offset so decal doesnt flicker
        Vector3 offset = normal * .01f;
        Quaternion rot = Quaternion.LookRotation(normal);
        
        //Spawn impact particles
        foreach (var impactParticle in impactParticlesPrefabs)
        {
            EffectsManager.Instance.SpawnParticlesAtPoint(impactParticle, position + offset, rot);
        }
    }

}
