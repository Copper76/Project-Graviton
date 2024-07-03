using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Visualization))]
public class GravityGun : MonoBehaviour
{
    [SerializeField] private float weaponRange = 30.0f;

    private Gravity _lookingObject;
    private Visualization _visualization;
    [SerializeField] private LayerMask arrowMask;
    [SerializeField] private LayerMask noArrowMask;

    private void Awake()
    {
        _visualization = GetComponent<Visualization>();
    }

    //TODO refactor this
    private void Update()
    {
        CheckTestObject();
    }

    private void CheckTestObject()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * weaponRange, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange, noArrowMask) && hit.collider.gameObject.TryGetComponent<Gravity>(out Gravity hitGravityComponet))
        {
            if (_lookingObject == hitGravityComponet) return;

            if (_lookingObject != null)
            {
                _lookingObject.LookAwayFromObject();
                _visualization.OnDeSelect();
            }
            _lookingObject = hitGravityComponet;
            _lookingObject.LookAtObject();
            _visualization.OnSelect(_lookingObject.transform);
        }
        else
        {
            if (_lookingObject != null)
            {
                _lookingObject.LookAwayFromObject();
                _visualization.OnDeSelect();
                _lookingObject = null;
            }
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fired");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange, arrowMask))
        {
            _lookingObject.SetGravityDir(_visualization.GetArrowDir(hit.collider.gameObject));
        }
    }

    public void AltFire(InputAction.CallbackContext context)
    {
        Debug.Log("Alt Fired");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange, arrowMask))
        {
            _lookingObject.SetGravityDir(_visualization.GetArrowDir(hit.collider.gameObject) * -1.0f);
        }
    }
}