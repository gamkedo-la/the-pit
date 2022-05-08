using UnityEngine;

namespace Graphics
{
    public class BackgroundBrightness : MonoBehaviour
    {
        public Color baseTint = Color.white;
        private Material backgroundMaterial;

        private static readonly int Tint = Shader.PropertyToID("_Tint");
        private const string PrefsKey = "Settings.BackgroundBrightness";
        
        public static float Value =>
            PlayerPrefs.GetFloat(PrefsKey,
                SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ? 0.8f : 0.6f);

        private void OnEnable()
        {
            backgroundMaterial = GetComponentInChildren<MeshRenderer>().material;
            SetMaterialProperties(Value);
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
            backgroundMaterial.SetColor(Tint, color);
        }
    }
}