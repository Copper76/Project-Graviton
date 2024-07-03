using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Show orientation UI for player to track their looking direction in x,y,z.
/// </summary>
public class OrientationManager : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    
    private void LateUpdate()
    {
        UpdateOrientation(_mainCamera.transform.rotation);
    }

    private void UpdateOrientation(Quaternion value)
    {
        transform.localRotation = Quaternion.Inverse(value);
        //Debug.DrawLine(transform.parent.position, (this.transform.position), Color.red);
    }
}
