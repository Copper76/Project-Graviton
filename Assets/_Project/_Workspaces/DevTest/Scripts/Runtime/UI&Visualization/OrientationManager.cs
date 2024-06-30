using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Show orientation UI for player to track their looking direction in x,y,z.
/// </summary>
public class OrientationManager : MonoBehaviour

{
    public static OrientationManager Instance;
    private Vector3 _pos;
    private Quaternion _rotateOffset;

    private void Awake()
    {
        Instance = this;
    }


    public void UpdateOrientation(Quaternion value)
    {
        transform.localRotation = Quaternion.Inverse(value);
        //Debug.DrawLine(transform.parent.position, (this.transform.position), Color.red);
    }
}
