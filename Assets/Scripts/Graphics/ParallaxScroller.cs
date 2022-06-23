using System;
using UnityEngine;

namespace Graphics
{
    public class ParallaxScroller : MonoBehaviour
    {
        public float baseSpeed;

        private ParallaxLayer[] layers;
        private float speedFactor;

        private void Start()
        {
            layers = GetComponentsInChildren<ParallaxLayer>();
            Array.Sort(layers, (r1, r2) => r2.relativeSpeed.CompareTo(r1.relativeSpeed));
            speedFactor = 1f / layers[0].relativeSpeed;
        }

        private void Update()
        {
            var delta = baseSpeed * Time.deltaTime * speedFactor;
            foreach (var layer in layers)
            {
                var t = layer.transform;
                t.transform.Translate(delta * layer.relativeSpeed, 0, 0);
            }
        }
    }
}