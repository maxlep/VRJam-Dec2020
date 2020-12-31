using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private UnityEvent onDeath;

    private float currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage);
    }
    
    [PunRPC]
    private void TakeDamageRPC(float damage)
    {
        if (isDead) return;
            
        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            onDeath.Invoke();
            isDead = true;
        }
    }
}
