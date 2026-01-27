using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlickering : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float flickerIntensity = 0.2f;
    [SerializeField] private float flickerPerSecond = 3f;
    [SerializeField] private float speedRandomness = 1f;
    
    
    private float time;
    private float startingIntensity;
    
    
    [Header("Objects")]
    [SerializeField] private Light2D light2d;

    private void OnValidate()
    {
        light2d = GetComponent<Light2D>();
        startingIntensity = light2d.intensity;
    }
    

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * (1-Random.Range(-speedRandomness, speedRandomness)) * Mathf.PI;
        light2d.intensity = startingIntensity + Mathf.Sin(time * flickerPerSecond) * flickerIntensity;
    }
}
