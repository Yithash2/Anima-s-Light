using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomLight : MonoBehaviour
{
    Light2D _light;

    private void OnValidate()
    {
        _light = GetComponent<Light2D>();
    }

    public Color Color { set { _light.color = value; } }

    public float Intensity { set { _light.intensity = value; } }

    public float InnerRad { set { _light.pointLightInnerRadius = value; } }
    public float OuterRad { set { _light.pointLightOuterRadius = value;} }
    public float FallofStr { set { _light.falloffIntensity = value; } }
}
