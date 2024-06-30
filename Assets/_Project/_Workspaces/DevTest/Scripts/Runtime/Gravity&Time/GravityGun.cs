using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGun : MonoBehaviour
{
    [SerializeField] private float weaponRange = 30.0f;

    private GameObject _manipulatedObject;
    private GameObject _lookingObject;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject != _lookingObject && hitObject.GetComponent<Gravity>() != null)
            {
                if (_lookingObject != null)
                {
                    //relinquish
                }
                _lookingObject = hitObject;

            }
        }
        else
        {
            if (_lookingObject != null)
            {
                //relinquish
            }
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fired");
    }
}
