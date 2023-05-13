using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject[] prefabList;
    private int maxTerrains = 3;
    private float spawn_X_Tr = 0;
    private float terrainLength = 20f;
    private Queue<GameObject> activeTerrains;
    private Queue<int> usedTerrainIndices;

    private void Start()
    {
        usedTerrainIndices = new Queue<int>();
        activeTerrains = new Queue<GameObject>();
        for (int i = 0; i < maxTerrains; i++)
        {
            SpawnTerrain();
        }
    }

    private void Update()
    {
        if (playerTransform.position.x > (activeTerrains.Peek().transform.position.x + terrainLength * 1.5))
        {
            DeleteTerrain();
            SpawnTerrain();
        }
    }

    private void SpawnTerrain()
    {
        int prefabIndex = Random.Range(0, prefabList.Length);
        while (usedTerrainIndices.Contains(prefabIndex))
        {
            prefabIndex = Random.Range(0, prefabList.Length);
        }
        GameObject prefab = Instantiate(prefabList[prefabIndex]);
        prefab.transform.SetParent(transform);
        prefab.transform.position = Vector3.right * spawn_X_Tr;
        spawn_X_Tr += terrainLength;
        usedTerrainIndices.Enqueue(prefabIndex);
        while (usedTerrainIndices.Count > 2)
        {
            usedTerrainIndices.Dequeue();
        }
        activeTerrains.Enqueue(prefab);
    }

    private void DeleteTerrain()
    {
        usedTerrainIndices.Dequeue();
        Destroy(activeTerrains.Dequeue());
    }
}