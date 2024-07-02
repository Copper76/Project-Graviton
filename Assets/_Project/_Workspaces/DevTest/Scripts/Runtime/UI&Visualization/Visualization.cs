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

    
    
    [SerializeField] private GameObject _selectedUIPref;
    
    private GameObject _currUI;

    private bool _isSelected;
    private GameObject _currTarget;



    //for rescale
    Bounds _bounds;
    Renderer _targetRenderer;
    Vector3 _center;
    Vector3 _extents;
    Vector3 _currSize;
    Quaternion _currRot;



    /// ////////////////////////////////////////////////////
    //TEST
    [SerializeField] private GameObject[] _selectedItemTest;
    [SerializeField] private GameObject _selectedItem;
    /// ////////////////////////////////////////////////////

    private void Awake()
    {
        _isSelected = false;
    }

    void Start()
    {
        
        _currUI = Instantiate(_selectedUIPref);
        _currUI.SetActive(false);

        ////////////////////////////////////////////////////////////////////
        //TEST
        StartCoroutine(testIt());
        
        ////////////////////////////////////////////////////////////////////

    }
    ////////////////////////////////////////////////////////////////////
    //Test
    private IEnumerator testIt()
    {
        
        for (int i = 0; i < _selectedItemTest.Length; i++)
        {

            OnSelect(_selectedItemTest[i].transform);
            yield return new WaitForSeconds(2f);
            _currUI.GetComponent<SelectedUI>().TestMovingDirection(_currTarget, 1f);

            yield return new WaitForSeconds(28f);
            OnDeSelect();
            yield return new WaitForSeconds(3f);
        }
    }
    ////////////////////////////////////////////////////////////////////



    private void LateUpdate()
    {
        if (!(_currUI.activeSelf) || _currTarget == null) return;

        //update UI position
        _currUI.transform.position = _currTarget.transform.position;


        //when target is selected, check if the target is resized/rotated in order to resize the arrow UI as well.
        if (_currSize != _currTarget.transform.localScale || _currRot != _currTarget.transform.rotation)
        {
            _currUI.GetComponent<SelectedUI>().ResizeUI(GetTargetBoundInfo());
            _currSize = _currTarget.transform.localScale;
            _currRot = _currTarget.transform.rotation;
        }


    }

    /// <summary>
    /// return a float list with info in order: disToPosX, disToNegX, disToPosY, disToNegY, disToPosZ, disToNegZ
    /// </summary>
    /// <returns></returns>
    private float[] GetTargetBoundInfo()
    {
        //Renderer is being used because i don't know what the collider looks like, Bill talked about add a bigger sphere collider for hover event.
        _targetRenderer = _currTarget.GetComponent<Renderer>();

        if (_targetRenderer == null) return null;

        _bounds = _targetRenderer.bounds;
        _extents = _bounds.extents;

        // Calculate distances from the center to each direction
        return new float[]{ _extents.x, -_extents.x, _extents.y, -_extents.y, _extents.z, -_extents.z};
    }

    public void OnSelect(Transform target)
    {
        
        _currTarget = target.gameObject;
        _currRot = target.rotation;
        _currSize = target.localScale;

        _currUI.SetActive(true);
        _currUI.GetComponent<SelectedUI>().ResizeUI(GetTargetBoundInfo());

        //TEST
        //_currUI.GetComponent<SelectedUI>().TestDirection();


    }

    public void OnDeSelect()
    {
        _currTarget = null;
        _currUI.SetActive(false);
    }

    

}
