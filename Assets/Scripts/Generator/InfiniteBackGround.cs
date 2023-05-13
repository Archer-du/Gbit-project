using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackGround : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject[] backgroundList;
    private int maxBackGround = 4;
    private float spawn_X_BG;
    private float backgroundLength = 16;
    private Queue<GameObject> activeBackGround;
    private Queue<int> usedBackGroundIndices;

    private void Start()
    {
        usedBackGroundIndices = new Queue<int>();
        activeBackGround = new Queue<GameObject>();
        spawn_X_BG = -2 * backgroundLength;
        for (int i = 0; i < maxBackGround; i++)
        {
            SpawnBackGround();
        }
    }

    private void Update()
    {
        if (playerTransform.position.x > (activeBackGround.Peek().transform.position.x + backgroundLength * 2.5f))
        {
            DeleteBackGround();
            SpawnBackGround();
        }
    }

    private void SpawnBackGround()
    {
        int BackGroundIndex = Random.Range(0, backgroundList.Length);
        while (usedBackGroundIndices.Contains(BackGroundIndex))
        {
            BackGroundIndex = Random.Range(0, backgroundList.Length);
        }
        GameObject prefab = Instantiate(backgroundList[BackGroundIndex]);
        prefab.transform.SetParent(transform);
        prefab.transform.position = Vector3.right * spawn_X_BG + Vector3.up * 4.5f;
        spawn_X_BG += backgroundLength;
        usedBackGroundIndices.Enqueue(BackGroundIndex);
        while (usedBackGroundIndices.Count > 1)
        {
            usedBackGroundIndices.Dequeue();
        }
        activeBackGround.Enqueue(prefab);
    }

    private void DeleteBackGround()
    {
        usedBackGroundIndices.Dequeue();
        Destroy(activeBackGround.Dequeue());
    }
}
