using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public void OnHover();
    public void Interact();
    public void OnHoverExit();
}
