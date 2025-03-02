using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHouse : MonoBehaviour
{
    [SerializeField] GameObject[] _housePrefabs;

    public int HouseIndex { set { if (value == -1) ChoseRandomTree(); else ReplaceObject(value); } }

    void ChoseRandomTree()
    {
        int i = Random.Range(0, _housePrefabs.Length);
        ReplaceObject(i);
    }

    void ReplaceObject(int i)
    {
        var go = _housePrefabs[i];

        if (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0));
        }

        Instantiate(go, transform);

    }
}
