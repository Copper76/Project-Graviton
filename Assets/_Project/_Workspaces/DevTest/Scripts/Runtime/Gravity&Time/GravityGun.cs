using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGun : MonoBehaviour
{
    [SerializeField] private float weaponRange = 30.0f;

    private GameObject _manipulatedObject;
    private Gravity _lookingObject;

    //TODO refactor this
    private void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * weaponRange, Color.green);
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange))
        {
            Gravity hitGravityComponet = hit.collider.gameObject.GetComponent<Gravity>();
            
            if (hitGravityComponet != null)
            {
                if (_lookingObject != null)
                {
                    if (_lookingObject != hitGravityComponet)
                    {
                        _lookingObject.LookAwayFromObject();
                        _lookingObject = hitGravityComponet;
                        _lookingObject.LookAtObject();
                    }
                }
                else
                {
                    _lookingObject = hitGravityComponet;
                    _lookingObject.LookAtObject();
                }
            }
        }
        else
        {
            if (_lookingObject != null)
            {
                _lookingObject.LookAwayFromObject();
                _lookingObject = null;
            }
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fired");
        RaycastHit hit;
        LayerMask arrowMask = LayerMask.GetMask("Arrow");
        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange, arrowMask))
        {
            Gravity hitObject = hit.collider.gameObject.GetComponent<Gravity>();
            if (hitObject != null)
            {
                hitObject.SetGravityDir(new Vector3(0.0f, 1.0f, 0.0f));
                //Change here after the arrows are implemented
            }
        }
    }
}
