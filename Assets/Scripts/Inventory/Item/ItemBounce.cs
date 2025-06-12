using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;

        private BoxCollider2D coll;

        public float gravity = -3.5f;

        private bool isGround;

        private float distance;

        private Vector2 direction;

        private Vector3 targetPos;


        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;
        }

        private void Update()
        {
            Bounce();
        }

        public void InitBounceItem(Vector3 target, Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = target;
            distance=Vector3.Distance(target, transform.position);

            spriteTrans.position += Vector3.up * 1.5f;
        }

        private void Bounce()
        {
            isGround=spriteTrans.position.y<=transform.position.y;

            //原本是大於0.01就需要接著移動，目前看來他無法檢查到他小於0.01，就先設置大點先。
            if(Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position += (Vector3)direction * distance  *-gravity * Time.deltaTime;
            }

            if(!isGround)
            {
                spriteTrans.position += Vector3.up * gravity * Time.deltaTime;
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;
            }
        }
    }
}

