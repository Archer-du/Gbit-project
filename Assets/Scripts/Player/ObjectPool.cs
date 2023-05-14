using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	public static ObjectPool instance;

	public GameObject shadows;
	public int shadowNum;
	private Queue<GameObject> shadowPool = new Queue<GameObject>();
	private void Awake()
	{
		instance = this;
		FillPool();
	}

	public void FillPool()
	{
		for(int i = 0; i < shadowNum; i++)
		{
			var newShadow = Instantiate(shadows);
			newShadow.transform.SetParent(transform);
			PoolReturn(newShadow);
		}
	}
	public void PoolReturn(GameObject shadow)
	{
		shadow.SetActive(false);
		shadowPool.Enqueue(shadow);
	}

	public GameObject PoolGet()
	{
		var shadow = shadowPool.Dequeue();
		shadow.SetActive(true);
		return shadow;
	}
}
