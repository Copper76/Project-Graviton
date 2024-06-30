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

    [SerializeField] private GameObject _selectedItem;

    [SerializeField] private Transform _selectedUIPref;


    private GameObject _currUI;




    // Start is called before the first frame update
    void Start()
    {

        //TEST
        OnSelect(_selectedItem.transform);
        //Invoke("OnDeSelect", 5f);


        
    }


    public void OnSelect(Transform target)
    {
        _currUI = Instantiate(_selectedUIPref.gameObject, target);
    }

    public void OnDeSelect()
    {
        Destroy(_currUI.gameObject);
    }


}
