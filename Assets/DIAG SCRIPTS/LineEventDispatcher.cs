using System.Collections;

namespace Dialogue
{
    using UnityEngine;

    public class LineEventDispatcher : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] audioClips;
        public void Dispatch(LineEventType eventType)
        {
            switch (eventType)
            {
                case LineEventType.StopMice :
                    GameObject.Find("Mice").GetComponent<Animator>().enabled = false;
                    break;
                case LineEventType.ShareCheese:
                    GameObject.Find("Cheese-weel").transform.GetChild(0).gameObject.SetActive(false);;
                    break;
                
                case LineEventType.BecomeCheese:
                    StartCoroutine(StayCheese());
                    break;
                
                case LineEventType.TeleportPlayerEasterEgg : 
                    var gameObj = new GameObject();
                    var transf = gameObj.transform;
                    transf.position = new Vector3(305.5f, 41.5f, 0);
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                    PlayerStateMachine.Instance.Teleport(transf);
                    break;
            }
        }

        private IEnumerator StayCheese()
        {
            PlayerStateMachine.Instance.GetComponent<Animator>().enabled = false;
            PlayerStateMachine.Instance.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Cheese_weel").GetComponentInChildren<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(120f);
            PlayerStateMachine.Instance.GetComponent<Animator>().enabled = true;
        }
    }
    

}