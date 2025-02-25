using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTimer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(WaitForEndOfFrameToStop());
    }

    IEnumerator WaitForEndOfFrameToStop()
    {
        yield return new WaitForEndOfFrame();
        SpeedrunClock.Instance.ChronoStopped = true;

    }
}
