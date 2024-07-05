using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDirection : MonoBehaviour
{
    [SerializeField] private Vector3 positiveDir;
    [SerializeField] private Vector3 negativeDir;

    public Vector3 GetPositiveDir()
    { 
        return positiveDir;
    }

    public Vector3 GetNegativeDir()
    {
        return negativeDir;
    }
}
