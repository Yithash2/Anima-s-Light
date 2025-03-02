using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMushroom : MonoBehaviour
{
    [SerializeField] GameObject[] _mushroomPrefabs;

    [field: SerializeField] public float Size { set { if (RandomSize) ChangeHeight(Random.Range(0,value)); else ChangeHeight(value); } }
    public int MushroomIndex { set { if (value == -1) ChoseRandomTree(); else ReplaceObject(value); } }

    public bool RandomSize { get; set; }

    GameObject _child;


    void ChoseRandomTree()
    {
        int i = Random.Range(0, _mushroomPrefabs.Length);
        ReplaceObject(i);
    }

    void ReplaceObject(int i)
    {
        _child = _mushroomPrefabs[i];

        if (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0));
        }

        _child = Instantiate(_child, transform);
    }

    void ChangeHeight(float value)
    {
        var sp = _child.GetComponent<SpriteRenderer>();

        Vector2 size = Vector2.up * value;
        Debug.Log(size);
        sp.size += size;
    }
}
