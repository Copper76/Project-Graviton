using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrientationManager : MonoBehaviour

{
    public static OrientationManager Instance;
    private void Awake()
    {
        Instance = this;
        _rotateOffset = Quaternion.LookRotation(_pos);
        transform.localRotation = _rotateOffset;
    }


    private Vector3 _pos;

    private Quaternion _rotateOffset;

    
   

    // Update is called once per frame
    public void UpdateOrientation(Quaternion value)
    {
        
        transform.localRotation = _rotateOffset * Quaternion.Inverse(value);
        Debug.DrawLine(transform.parent.position, (this.transform.position), Color.red);
            //(transform.parent.position, _pos * 1.2f, Color.red);
    }
}
