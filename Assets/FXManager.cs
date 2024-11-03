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
        _speedLineAlpha = _speedLineImage.color.a;
        _hueShiftMaterial.TryGet(out _hueAdjustments);
        _hueAdjustments.hueShift.value = 0f;
    }

    public void SpeedLines(bool active, Rigidbody2D _rb){

        Vector2 direction = FindClosestVector(_rb.velocity.normalized); // Replace with your 2D direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _speedLineFX.transform.rotation = Quaternion.Euler(0, 0, angle);

        if(!active && !_speedLineRegression && _speedLineImage.color.a != 0){
            StartCoroutine(SpeedLines(_rb));
        }else if(active){
            _speedLineFX.SetActive(active);
        }

    }

    IEnumerator SpeedLines(Rigidbody2D _rb){
        _speedLineRegression = true;

        

        for(float t = 0f; t < 1f; t += Time.deltaTime){
            float lerpedAlpha = Mathf.Lerp(_speedLineAlpha, 0, t/1f);
            _speedLineImage.color = new Color(_speedLineImage.color.r, _speedLineImage.color.g, _speedLineImage.color.b, lerpedAlpha);

            if(_rb.velocity.magnitude > 40){
                _speedLineImage.color = new Color(_speedLineImage.color.r, _speedLineImage.color.g, _speedLineImage.color.b, _speedLineAlpha);
                _speedLineRegression = false;
                StopCoroutine(SpeedLines(_rb));
            }

            
            yield return null;
        }

        _speedLineRegression = false;
        _speedLineFX.SetActive(false);
        _speedLineImage.color = new Color(_speedLineImage.color.r, _speedLineImage.color.g, _speedLineImage.color.b, _speedLineAlpha);

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

    Vector2 FindClosestVector(Vector2 direction)
    {
        Vector2 closestVector = Vector2.zero;
        float maxDot = float.MinValue;

        direction = direction.normalized;

        foreach (Vector2 vector in _possibleDirections)
        {
            float dotProduct = Vector2.Dot(vector.normalized, direction);

            if (dotProduct > maxDot)
            {
                maxDot = dotProduct;
                closestVector = vector;
            }
        }

        return closestVector;
    }

    Vector2[] _possibleDirections = new Vector2[] {
                                                    Vector2.up,
                                                    Vector2.down,
                                                    Vector2.left,
                                                    Vector2.right
                                                    //new Vector2(1, 1).normalized,
                                                    //new Vector2(-1, 1).normalized,
                                                    //new Vector2(-1, -1).normalized,
                                                    //new Vector2(1, -1).normalized
                                                    };

    bool _speedLineRegression;
    float _speedLineAlpha;
    [SerializeField] GameObject _speedLineFX;
    [SerializeField] Image _speedLineImage;
    [SerializeField] GameObject _waterTintFX;

    [SerializeField] VolumeProfile _hueShiftMaterial;
    ColorAdjustments _hueAdjustments;
}
