using UnityEngine;

namespace Objects
{
    public class CameraBounds : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider;
        
        public float Height
        {
            set
            {
                boxCollider.size = new Vector2(boxCollider.size.x, value/16);
                transform.position -= new Vector3(0, value/32);
            }
        }
        
        public float Length
        {
            set
            {
                boxCollider.size = new Vector2(value/16, boxCollider.size.y);
                transform.position += new Vector3(value/32, 0);
            }
        }
        
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