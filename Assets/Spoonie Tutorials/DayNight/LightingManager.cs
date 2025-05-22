using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    // Scene References
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;

    [Header("Time Settings")]
    [Tooltip("Current time of day (0 = midnight, 12 = noon, 24 = next midnight)")]
    [SerializeField, Range(0, 24)] private float timeOfDay = 0f;

    [Tooltip("Length of a full 24-hour day in real-time seconds")]
    public float dayDurationInSeconds = 60f;

    private void Update()
    {
        if (preset == null)
            return;

        if (Application.isPlaying)
        {
            float timeDelta = Time.deltaTime / dayDurationInSeconds * 24f;
            timeOfDay = (timeOfDay + timeDelta) % 24f;
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        if (preset == null)
            return;

        // Set ambient and fog colors
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        // Update directional light
        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    // Optionally allow setting time externally in a safe way
    public void SetTimeOfDay(float newTime)
    {
        timeOfDay = Mathf.Repeat(newTime, 24f);
        UpdateLighting(timeOfDay / 24f);
    }




    // //Try to find a directional light to use if we haven't set one
    // private void OnValidate()
    // {
    //     if (DirectionalLight != null)
    //         return;

    //     //Search for lighting tab sun
    //     if (RenderSettings.sun != null)
    //     {
    //         DirectionalLight = RenderSettings.sun;
    //     }
    //     //Search scene for light that fits criteria (directional)
    //     else
    //     {
    //         Light[] lights = GameObject.FindObjectsOfType<Light>();
    //         foreach (Light light in lights)
    //         {
    //             if (light.type == LightType.Directional)
    //             {
    //                 DirectionalLight = light;
    //                 return;
    //             }
    //         }
    //     }
    // }

}
