using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTool_fpsLimiter : MonoBehaviour
{
    [SerializeField, Range(10f, 200f)] int _fpsLimit = 120;
    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = _fpsLimit;
    }
}
