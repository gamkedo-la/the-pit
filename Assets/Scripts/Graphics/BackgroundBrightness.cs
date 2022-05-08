using UnityEngine;

namespace Graphics
{
    public class BackgroundBrightness : MonoBehaviour
    {
        public static readonly string PrefsKey = "Settings.BackgroundBrightness";
        
        public Color baseTint = Color.white;
        [Range(0f, 1f)]
        public float defaultBrightness = 0.6f;
        private Material backgroundMaterial;

        private void OnEnable()
        {
            backgroundMaterial = GetComponentInChildren<MeshRenderer>().material;
            SetMaterialProperties(PlayerPrefs.GetFloat(PrefsKey, defaultBrightness));
        }

        public void SetBackgroundBrightness(float value)
        {
            SetMaterialProperties(value);
            PlayerPrefs.SetFloat(PrefsKey, value);
        }

        private void SetMaterialProperties(float value)
        {
            var color = baseTint;
            color *= value;
            backgroundMaterial.SetColor("_Tint", color);
        }
    }
}