using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FollowSelected : MonoBehaviour
{
    EventSystem _es;
    Camera _cam;

    [SerializeField] Transform _maxLeft;
    [SerializeField] Transform _maxRight;
    void Start()
    {
        _cam = Camera.main;
        _es = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {   
        if(_es.currentSelectedGameObject != null){
            RectTransform _target = _es.currentSelectedGameObject.GetComponent<RectTransform>();
            Vector3 _targetWP = _target.transform.position;
            transform.position = Vector3.Lerp(transform.position,_targetWP + new Vector3(0, 60, -10), 0.5f);
        }else{
            
            if(Mouse.current.position.ReadValue().x < Screen.width / 4f){
                transform.position += Vector3.left * 320f * Time.deltaTime;
            }
            else if (Mouse.current.position.ReadValue().x > Screen.width * 3f / 4f){
                transform.position += Vector3.right * 320f * Time.deltaTime;
            }
            
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _maxLeft.position.x, _maxRight.position.x), transform.position.y, transform.position.z);
        }
    }

    
}
