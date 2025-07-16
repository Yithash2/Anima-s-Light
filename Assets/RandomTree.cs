using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTree : MonoBehaviour
{
    [SerializeField] GameObject[] _treePrefabs;

    public int TreeIndex { set { if (value == -1) ChoseRandomTree(); else ReplaceObject(value); } }

    public bool InFront { private get; set; }

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

        go = Instantiate(go, transform);

        if(InFront)
        ChangeSortOrder(go.transform);
    }

    void ChangeSortOrder(Transform tree)
    {
        var trunk = tree.GetChild(0);
        var leaves = tree.GetChild(1);

        trunk.GetComponent<SpriteRenderer>().sortingOrder = -5;
        leaves.GetComponent<SpriteRenderer>().sortingOrder = -4;
    }
}
