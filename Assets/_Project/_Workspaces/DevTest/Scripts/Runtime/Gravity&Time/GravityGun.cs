using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGun : MonoBehaviour
{

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fired");
    }
}
