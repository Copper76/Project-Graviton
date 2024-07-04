using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


/// <summary>
/// Manage the UI when player select a game object, show gizmos on game object for player to drag.
/// At most 1 object can be selected.
/// </summary>
/// 

public class Visualization : MonoBehaviour
{
    [SerializeField] private GameObject selectedUIPref;
    
    private SelectedUI _currUI;

    private GameObject _currTarget;

    //for rescale
    private Vector3 _currSize;
    private Quaternion _currRot;

    //Maintain visual size
    public Camera mainCamera;
    public float initialScale = 1f; // Adjust this to the desired size

    private void Awake()
    {

    }

    void Start()
    {
        _currUI = Instantiate(selectedUIPref).GetComponent<SelectedUI>();
        _currUI.gameObject.SetActive(false);

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        SelectedUITransformUpdate();
    }

    private void SelectedUITransformUpdate()
    {
        if (!_currUI.gameObject.activeSelf) return;

        //update UI position
        _currUI.transform.position = _currTarget.transform.position;


        //when target is selected, check if the target is resized/rotated in order to resize the arrow UI as well.
        if (_currSize != _currTarget.transform.localScale || _currRot != _currTarget.transform.rotation)
        {
            _currUI.ResizeUI(GetTargetBoundInfo());
            _currSize = _currTarget.transform.localScale;
            _currRot = _currTarget.transform.rotation;
        }

        //reset size depends on distance to player
        float distance = Vector3.Distance(_currTarget.transform.position, mainCamera.transform.position);
        _currUI.transform.localScale = Vector3.one * initialScale * Mathf.Max(1.0f, distance / 5); //current ratio: length of arrow/5 = new length/curr Distance
    }

    /// <summary>
    /// return a float list with info in order: disToPosX, disToNegX, disToPosY, disToNegY, disToPosZ, disToNegZ
    /// </summary>
    /// <returns></returns>
    private float[] GetTargetBoundInfo()
    {
        //Renderer is being used because i don't know what the collider looks like, Bill talked about add a bigger sphere collider for hover event.

        if (!_currTarget.TryGetComponent<Renderer>(out Renderer targetRenderer)) return null;

        Vector3 extents = targetRenderer.bounds.extents;

        // Calculate distances from the center to each direction
        return new float[]{ extents.x, -extents.x, extents.y, -extents.y, extents.z, -extents.z};
    }

    public void OnSelect(Transform target)
    {
        
        _currTarget = target.gameObject;
        _currRot = target.rotation;
        _currSize = target.localScale;

        _currUI.gameObject.SetActive(true);
        _currUI.ResizeUI(GetTargetBoundInfo());
    }

    public void OnDeSelect()
    {
        _currUI.gameObject.SetActive(false);
    }

    public Vector3 GetArrowDir(int dirIndex)
    {
        return _currUI.OnDirectionPicked(dirIndex);
    }
}