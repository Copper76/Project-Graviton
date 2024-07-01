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
    [SerializeField] private GameObject _selectedUIPref;
    
    private GameObject _currUI;
    

    // Start is called before the first frame update
    void Start()
    {

        //TEST
        OnSelect(_selectedItem.transform);
        //Invoke("OnDeSelect", 5f);
        
        _currUI = Instantiate(_selectedUIPref);
        _currUI.SetActive(false);
    }

    public void OnSelect(Transform target)
    {
        _currUI.SetActive(true);
    }

    public void OnDeSelect()
    {
        _currUI.SetActive(false);
    }

    

}
