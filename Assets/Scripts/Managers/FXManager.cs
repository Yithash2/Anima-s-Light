using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance {get; protected set;}
    void Awake()
    {
        if(Instance != null){
            Debug.Log("Found more then one TotalDeathCounter, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start(){
        _hueShiftMaterial.TryGet(out _hueAdjustments);
        _hueAdjustments.hueShift.value = 0f;
    }

    public void WaterTint(bool active){
        _waterTintFX.SetActive(active);
        
    }

    public void LerpHue(float lerpValue){


        lerpValue = Mathf.Clamp(lerpValue, 0, 1);
        
        float _targetHue = 95f;
        float _originalHue = 0f;

        
        float _hue = Mathf.Lerp(_targetHue,_originalHue, lerpValue);
        
        _hueAdjustments.hueShift.value = _hue;
    }

    [SerializeField] Image _speedLineImage;
    [SerializeField] GameObject _waterTintFX;

    [SerializeField] VolumeProfile _hueShiftMaterial;
    ColorAdjustments _hueAdjustments;
}
