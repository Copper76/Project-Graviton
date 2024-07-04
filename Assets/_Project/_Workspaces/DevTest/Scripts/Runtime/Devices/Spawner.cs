using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 rotationOffsetEuler;
    [SerializeField] private GameObject spawnPrefab;

    [SerializeField] private bool limitedSpawns = false;
    [SerializeField] private int maxSpawnedObjects = 1;

    private List<GameObject> spawnedObjects;
    private Quaternion _rotationOffset;

    private void Awake()
    {
        spawnedObjects = new List<GameObject>();
        _rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }

    public void Spawn()
    {
        if (limitedSpawns)
        {
            CleanList();

            GameObject newObject;
            if (spawnedObjects.Count == maxSpawnedObjects)
            {
                newObject = spawnedObjects[0]; // Take the object that was the spawned first
                newObject.transform.position = gameObject.transform.position + spawnOffset;
                newObject.transform.rotation = gameObject.transform.rotation * _rotationOffset;
                spawnedObjects.RemoveAt(0);
            }
            else
            {
                newObject = Instantiate(spawnPrefab, gameObject.transform.position + spawnOffset, gameObject.transform.rotation * _rotationOffset);
            }
            spawnedObjects.Add(newObject);
        }
        else
        {
            Instantiate(spawnPrefab, gameObject.transform.position + spawnOffset, gameObject.transform.rotation * _rotationOffset);
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
