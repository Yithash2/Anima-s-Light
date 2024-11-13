using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForceSceneTransition : MonoBehaviour
{
    [SerializeField]
    string _sceneToLoad;

    void OnTriggerEnter2D(Collider2D collider){
        SceneTransition.Instance.TransitionScene(_sceneToLoad);
    }
}
