using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private GameObject spawnPrefab;

    [SerializeField] private bool limitedSpawns = false;
    [SerializeField] private int maxSpawnedObjects = 1;

    private List<GameObject> spawnedObjects;

    private void Awake()
    {
        spawnedObjects = new List<GameObject>();
    }

    public void Spawn()
    {
        if (limitedSpawns)
        {
            CleanList();

            GameObject newObject;
            if (spawnedObjects.Count == maxSpawnedObjects)
            {
                Destroy(spawnedObjects[0]); // Take the object that was the spawned first
                spawnedObjects.RemoveAt(0);
            }
            newObject = Instantiate(spawnPrefab, spawnTransform.position, spawnTransform.rotation);
            spawnedObjects.Add(newObject);
        }
        else
        {
            Instantiate(spawnPrefab, spawnTransform.position, spawnTransform.rotation);
        }
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
