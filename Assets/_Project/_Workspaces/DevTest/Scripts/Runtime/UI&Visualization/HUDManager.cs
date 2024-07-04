using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject _gunPointerUI;

   
    public void ActiveHUD()
    {
        _gunPointerUI.SetActive(true);
    }
    public void InactiveHUD()
    {
        _gunPointerUI.SetActive(false);
    }
}
