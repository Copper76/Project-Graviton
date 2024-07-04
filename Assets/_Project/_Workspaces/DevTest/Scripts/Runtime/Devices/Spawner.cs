using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private GameObject spawnPrefab;

    [SerializeField] private int maxSpawnedObjects;

    private List<GameObject> spawnedObjects;

    private void Awake()
    {
        spawnedObjects = new List<GameObject>();
    }

    public void Spawn()
    {
        CleanList();

        GameObject newObject;
        if (spawnedObjects.Count == maxSpawnedObjects)
        {
            newObject = spawnedObjects[0];
            spawnedObjects.RemoveAt(0);
            newObject.transform.position = gameObject.transform.position + spawnOffset;
            newObject.transform.rotation = gameObject.transform.rotation;
        }
        else
        {
            newObject = Instantiate(spawnPrefab, gameObject.transform.position + spawnOffset, gameObject.transform.rotation);
        }
        spawnedObjects.Add(newObject);
    }

    private void CleanList()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}
