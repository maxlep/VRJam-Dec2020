using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Photon.Pun;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private string enemyLayer = "Enemy";
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask fireMask;
    [SerializeField] private float shootParticleSpeed = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private GameObject shootParticlesPrefab;
    [SerializeField] private List<GameObject> impactParticlesPrefabs;

    public void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit,
            Mathf.Infinity, fireMask))
        {
            //TODO: Check layer and deal damage
            GameObject other = hit.collider.gameObject;
            if (other.layer == LayerMask.NameToLayer(enemyLayer))
            {
                DamageProxy damageProxy = other.GetComponent<DamageProxy>();
                damageProxy.SendDamage(damage);
            }
            
            //Spawn particles with initial speed & lifetime until impact then spawn impact particle on destroy
            float impactTime = hit.distance / shootParticleSpeed;
            photonView.RPC("SpawnShootParticlesInit", RpcTarget.AllBuffered, impactTime, 
                hit.point, hit.normal);
        }
        else
        {
            //Spawn particles with speed default lifetime
            //SpawnShootParticles();
            photonView.RPC("SpawnShootParticles", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void SpawnShootParticles()
    {
        GameObject shootParticles = EffectsManager.Instance.SpawnParticlesAtPoint(shootParticlesPrefab,
            firePoint.position, firePoint.rotation, true, 5f);
        shootParticles.GetComponent<RaycastProjectile>().Init(shootParticleSpeed);
    }
    
    [PunRPC]
    public void SpawnShootParticlesInit(float lifeTime, Vector3 hitPoint, Vector3 hitNormal)
    {
        GameObject shootParticles = EffectsManager.Instance.SpawnParticlesAtPoint(shootParticlesPrefab,
            firePoint.position, firePoint.rotation, true, lifeTime);
        shootParticles.GetComponent<RaycastProjectile>().Init(shootParticleSpeed);
        
        RaycastProjectile projectileScript = shootParticles.GetComponent<RaycastProjectile>();
        projectileScript.Init(shootParticleSpeed);
        projectileScript.onDestroyed += () => SpawnImpactParticles(hitPoint, hitNormal);
    }

    [PunRPC]
    public void SpawnImpactParticles(Vector3 position, Vector3 normal)
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
