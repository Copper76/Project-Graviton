using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDirection : MonoBehaviour
{
    [SerializeField] private int positiveDirIndex;
    [SerializeField] private int negativeDirIndex;

    public int GetPositiveDirIndex()
    { 
        return positiveDirIndex;
    }

    public int GetNegativeDirIndex()
    {
        return negativeDirIndex;
    }
}
