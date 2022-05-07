using UnityEngine;

namespace Graphics
{
    public class BackgroundBrightness : MonoBehaviour
    {
        public Color baseTint = Color.white;
        private Material backgroundMaterial;

        private void OnEnable()
        {
            backgroundMaterial = GetComponentInChildren<MeshRenderer>().material;
        }

        public void SetBackgroundBrightness(float value)
        {
            var color = baseTint;
            color *= value;
            backgroundMaterial.SetColor("_Tint", color);
        }
    }
}