using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerProxy : MonoBehaviour
{
    [SerializeField] private string layerName = "Player";

    private List<GameObject> objectsInRange = new List<GameObject>();

    public List<GameObject> ObjectsInRange => objectsInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            objectsInRange.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            objectsInRange.Remove(other.gameObject);
        }
    }
}
