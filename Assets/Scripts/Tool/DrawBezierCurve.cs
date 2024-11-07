using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DrawBezierCurve : MonoBehaviour
{
    public Transform[] ControlPoints;

    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        for(float t = 0;t <= 1; t += 0.01f){
            Vector3 _position = Vector3.zero;
            for(int i = 0; i < ControlPoints.Length; i++){
                float bionomialCoeff = BinomialCoefficient(ControlPoints.Length - 1, i);
                float term = bionomialCoeff = Mathf.Pow(t, i) * Mathf.Pow(1- t, ControlPoints.Length - 1 - i);

                _position += term * ControlPoints[i].position;
            }

            Gizmos.DrawSphere(_position, 0.25f);
        }
    }

    static int BinomialCoefficient(int n, int k){
            int result = 1;
            for(int i = 1; i <= k; i++){
                result *= (n - i + 1);
                result /= i;
            }
            return result;
    }

    Vector3[] GetChilds(Transform transform){
        List<Vector3> result = new List<Vector3>();
        int count = transform.childCount;

        for(int i = 0; i < count; i++){
            result.Append(transform.GetChild(i).position);
        }

        return result.ToArray();
            
    }
}
