using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float fallDistanceCutOff;
    [SerializeField] private Transform respawnPoint;

    void Update()
    {
        if (transform.position.y > fallDistanceCutOff) return;

        transform.position = respawnPoint.position;
    }
}
