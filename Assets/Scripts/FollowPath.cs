using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    
    [SerializeField] Transform[] path;
    int i;
    [SerializeField] bool start;

    void Start()
    {
        i = path.Length;
    }

    // Update is called once per frame
    void Update()
    
    {


        if(start){
        if(i != 0){
            transform.position = Vector3.Lerp(transform.position, path[i-1].position, 0.0125f);

            if(Vector3.Distance(transform.position, path[i-1].position) < 1) i --;
        }}
    }
}
