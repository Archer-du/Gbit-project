using UnityEngine;
using System.Collections.Generic;
using GbitProjectControl;
using GbitProjectState;
using EnemyController;
public class BulletPool : MonoBehaviour
{
    // 单例模式，方便外部访问
    public static BulletPool Instance { get; private set; }

    // 预制体，用来实例化子弹对象
    public GameObject bulletPrefab;

    // 池的大小，即最多可以存放多少个子弹对象
    public int poolSize = 10;

    // 用一个队列来存放空闲的子弹对象
    private Queue<Bullet> pool;

    private void Awake()
    {
        // 初始化单例
        Instance = this;

        // 初始化队列
        pool = new Queue<Bullet>();

        // 实例化子弹对象，并加入到队列中
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.SetActive(false); // 初始时设置为不激活
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            pool.Enqueue(bullet); // 加入到队列中
        }
    }

    // 从池中获取一个子弹对象，如果池为空，则返回 null
    public Bullet GetFromPool()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue(); // 从队列头部取出一个子弹对象
        }
        else
        {
            return null;
        }
    }

    // 将一个子弹对象归还到池中，如果池已满，则销毁该对象
    public void ReturnToPool(Bullet bullet)
    {
        if (pool.Count < poolSize)
        {
            bullet.gameObject.SetActive(false); // 设置为不激活
            pool.Enqueue(bullet); // 加入到队列尾部
        }
        else
        {
            Destroy(bullet.gameObject); // 销毁该对象
        }
    }
}
