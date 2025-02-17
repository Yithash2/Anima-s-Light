using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTimer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpeedrunClock.Instance.ChronoStopped = true;
    }
}
