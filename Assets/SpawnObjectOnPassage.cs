using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnPassage : MonoBehaviour
{
    [SerializeField] DetectPlayerPassage _sensor;

    [SerializeField] GameObject ObjectSpawned;

    [SerializeField] Vector2 _ofset;

    private void OnValidate()
    {
        _sensor = GetComponent<DetectPlayerPassage>();
    }

    private void OnEnable()
    {
        _sensor.OnEnougthPlayerPassage += SpawnObject;
    }

    private void OnDisable()
    {
        _sensor.OnEnougthPlayerPassage -= SpawnObject;
    }

    private void SpawnObject()
    {
        Instantiate(ObjectSpawned, transform.position + (Vector3)_ofset, Quaternion.identity);
    }
}
