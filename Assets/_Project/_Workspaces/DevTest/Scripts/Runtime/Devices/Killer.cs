using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    [SerializeField] private Vector3 spawnLocation;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.position = spawnLocation;
            collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
