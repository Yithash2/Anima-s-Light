using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Level level;

    [SerializeField] GameObject LevelInf;

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        Instantiate(LevelInf);

        LevelInfo.Instance.level = level;

        SceneManager.LoadScene(level.LevelSceneName);
    }
}
