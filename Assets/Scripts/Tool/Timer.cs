using UnityEngine;

namespace Tool
{
    public class Timer
    {
        public float Duraction {private set; get; }
        private float _end;
        

        public Timer(float duration)
        {
            Duraction = duration;
            _end = Time.time + duration;
        }
        
        public bool Ended() => Time.time >= _end;

        public void Reset()
        {
            _end = Time.time + Duraction;
        }
    }
}