using UnityEngine;

[System.Serializable]
public struct MovingPlatformPoints
{
    public Transform transform;

    public float WaitTime;

    public bool IsCurved;

    public bool IsEqual(MovingPlatformPoints otherPoint){
        if((this.transform.position == otherPoint.transform.position)&&(this.WaitTime == otherPoint.WaitTime)){
            return true;
        }else{
            return false;
        }
            
    }
}