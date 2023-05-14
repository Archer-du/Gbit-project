//author: Archer
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GbitProjectCore
{
    public class ShadowController : MonoBehaviour
    {
        private Transform playerTransform;

		[SerializeField] private SpriteRenderer shadowSprite;
        [SerializeField] private SpriteRenderer playerSprite;
		[SerializeField] private Color shadowColor;
		[SerializeField] private float shadowAlpha;
		[SerializeField] private float shadowAlphaMultiplier;
		[SerializeField] private float activeTimer;
		[SerializeField] private float activeTimeWindow;
        [SerializeField] private int counter;

		private void Awake()
		{
            shadowAlphaMultiplier = 1f;
            activeTimeWindow = 1f;
		}
		private void OnEnable()
		{
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            shadowSprite = GetComponent<SpriteRenderer>();
            playerSprite = playerTransform.GetComponent<SpriteRenderer>();
            shadowAlpha = 0.75f;
            activeTimer = 0;

            shadowSprite.sprite = playerSprite.sprite;

            transform.position = playerTransform.position;
            transform.rotation = playerTransform.rotation;
            transform.localScale = playerTransform.localScale;
		}
		void FixedUpdate()
        {
            shadowAlpha *= shadowAlphaMultiplier;
            shadowColor = new Color(0, 0, 1, shadowAlpha);
            shadowSprite.color = shadowColor;
            if(activeTimer < activeTimeWindow)
            {
                activeTimer += Time.fixedDeltaTime;
            }
			else// return to object pool
			{
                ObjectPool.instance.PoolReturn(this.gameObject);
            }
        }
    }

}
