using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Visualization))]
public class GravityGun : MonoBehaviour
{
    [SerializeField] private float weaponRange = 30.0f;
    [SerializeField] private LayerMask arrowMask;
    [SerializeField] private LayerMask noArrowMask;

    private Gravity _lookingObject;
    private GameObject _lookingArrow;
    private Visualization _visualization;

    [SerializeField] private float _fireRate = 1.0f;
    private float _fireRateCounter = 0.0f;

    private void Awake()
    {
        _visualization = GetComponent<Visualization>();
    }

    //TODO refactor this
    private void Update()
    {
        CheckTestObject();
        UpdateFireTimer();
    }

    private void UpdateFireTimer()
    {
        if (_fireRateCounter > 0.0f)
        {
            _fireRateCounter -= Time.deltaTime;
        }
        else
        {
            _fireRateCounter = 0.0f;
        }
    }

    private void CheckTestObject()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * weaponRange, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange, arrowMask))
        {
            _lookingArrow = hit.transform.gameObject;
        }
        else
        {
            _lookingArrow = null;
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange, noArrowMask) 
            && hit.collider.gameObject.TryGetComponent<Gravity>(out Gravity hitGravityComponet))
        {
            if (_lookingObject == hitGravityComponet) return;

            if (_lookingObject != null)
            {
                _lookingObject.LookAwayFromObject();
                _visualization.OnDeSelect();
            }
            _lookingObject = hitGravityComponet;
            _lookingObject.LookAtObject();
            _visualization.UpdateArrowVisual(_lookingObject.GetGravityDir());
            _visualization.OnSelect(_lookingObject.transform);
        }
        else
        {
            if (_lookingObject != null && _lookingArrow == null)
            {
                _lookingObject.LookAwayFromObject();
                _visualization.OnDeSelect();
                _lookingObject = null;
            }
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (_fireRateCounter > 0.0f) return;

        _fireRateCounter = _fireRate;

        if (_lookingArrow)
        {
            Vector3 gravityDir = _lookingArrow.GetComponent<GravityDirection>().GetPositiveDir();
            _lookingObject.SetGravityDir(gravityDir);
            _visualization.UpdateArrowVisual(gravityDir);
        }
    }

    public void AltFire(InputAction.CallbackContext context)
    {
        if (_fireRateCounter > 0.0f) return;

        _fireRateCounter = _fireRate;

        if (_lookingArrow)
        {
            Vector3 gravityDir = _lookingArrow.GetComponent<GravityDirection>().GetNegativeDir();
            _lookingObject.SetGravityDir(gravityDir);
            _visualization.UpdateArrowVisual(gravityDir);
        }
    }
}