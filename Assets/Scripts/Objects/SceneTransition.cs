using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    [SerializeField]
    Animator _animator;

    public static SceneTransition Instance {get; protected set;}
    void Awake(){
        if(Instance != null){
            Debug.Log("Found more then one DialogueManager, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void TransitionScene(string sceneName){
        StartCoroutine(PlaySceneTransition(sceneName));
    }

    public void TransitionScene(int sceneBuildIndex){

    }

    IEnumerator PlaySceneTransition(string sceneName){
        
        _animator.SetBool("Start", true);
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene(sceneName);
        _animator.SetBool("Start", false);
    }
}
