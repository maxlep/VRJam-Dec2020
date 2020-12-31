using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageProxy : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> onSendDamage;

    public void SendDamage(float damage)
    {
        onSendDamage.Invoke(damage);
    }
}
