using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MyAssets.Scripts.Interactions
{
    public class ResizePlayer : MonoBehaviour
    {
        [SerializeField] private List<Transform> scaleTransforms = new List<Transform>();
        
        public void ScaleUp(float rate)
        {
            foreach (var scaleTransform in scaleTransforms)
            {
                scaleTransform.localScale += Vector3.one * rate * Time.deltaTime;
            }
        }
    }
}