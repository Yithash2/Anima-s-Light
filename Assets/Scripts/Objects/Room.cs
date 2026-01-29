using UnityEngine;

namespace Objects
{
    public class CamRoom : MonoBehaviour
    {
        public GameObject virtualCamera;
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                virtualCamera.SetActive(true);
        }
        
        void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                virtualCamera.SetActive(false);
        }
    }
}