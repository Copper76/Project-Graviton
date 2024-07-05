using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    [SerializeField] private Transform spawnTransform;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.position = spawnTransform.position;
            collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
