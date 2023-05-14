using UnityEngine;
using System.Collections.Generic;
public class BulletPool : MonoBehaviour
{
    
    public static BulletPool Instance { get; private set; }

    
    public GameObject bulletPrefab;

    
    public int poolSize = 10;

    
    private Queue<Bullet> pool;

    private void Awake()
    {
        
        Instance = this;
        pool = new Queue<Bullet>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.SetParent(transform);
            bulletObject.SetActive(false); 
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            pool.Enqueue(bullet); 
        }
    }

    public Bullet GetFromPool()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue(); 
        }
        else
        {
            return null;
        }
    }

    public void ReturnToPool(Bullet bullet)
    {
        if (pool.Count < poolSize)
        {
            bullet.gameObject.SetActive(false); 
            pool.Enqueue(bullet); 
        }
        else
        {
            Destroy(bullet.gameObject); 
        }
    }
}
