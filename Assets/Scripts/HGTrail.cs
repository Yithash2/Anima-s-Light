using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Higher
{
    public class HGTrail : MonoBehaviour
    {
        private float DelayinSec;
        private GameObject[] nGhost;
        private float AlphaValue = 0.5f;
        private SpriteRenderer GSprite;
        
        private float trailDelay = 0.1f;
        private Transform trailPlayer;
        private SpriteRenderer trailSpriteTarget;

        private void Start()
        {
            trailPlayer = gameObject.transform;
            trailSpriteTarget = gameObject.GetComponent<SpriteRenderer>();
            DelayinSec = trailDelay;
        }

        private void perFrame()
        {
            if (DelayinSec > 0)
            {
                DelayinSec -= Time.deltaTime;
            }
            else
            {
                StartCoroutine(nInstance());
                DelayinSec = trailDelay;
            }
        }

        private void Update()
        {
            perFrame();
        }

        private IEnumerator nInstance()
        {
            nGhost = new GameObject[] { new GameObject("Ghost") };

            foreach (var i in nGhost)
            {
                GSprite = i.AddComponent<SpriteRenderer>();
                GSprite.sprite = trailSpriteTarget.sprite;
                GSprite.color = new Vector4(255, 255, 255, AlphaValue);
                GSprite.transform.position = trailSpriteTarget.transform.position;
                GSprite.sortingOrder = 10;

                if (trailPlayer.transform.localScale.x < 0)
                {
                    GSprite.flipX = true;
                }
                else
                {
                    GSprite.flipX = false;
                }

                yield return new WaitForSeconds(0.35f);
                Destroy(i);
            }
        }
    }
}
