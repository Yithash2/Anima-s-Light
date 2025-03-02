using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTree : MonoBehaviour
{
    [SerializeField] GameObject[] _treePrefabs;

    public int TreeIndex { set { if (value == -1) ChoseRandomTree(); else ReplaceObject(value); } }

    void ChoseRandomTree()
    {
        int i = Random.Range(0, _treePrefabs.Length);
        ReplaceObject(i);
    }

    void ReplaceObject(int i)
    {
        var go = _treePrefabs[i];

        if (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0));
        }

        Instantiate(go, transform);

    }
}
