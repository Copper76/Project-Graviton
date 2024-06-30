using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectedUI : MonoBehaviour
{

    [SerializeField] private Transform[] _Dir;

    /// <summary>
    /// Invisible other gizmo direction UI
    /// </summary>
    public void OnDirectionPicked(GameObject dir)
    {
        for (int i = 0; i < _Dir.Length; i++)
        {
            if (_Dir[i] != dir) _Dir[i].gameObject.SetActive(false);
        }

        //TODO: move correspondingly by player interaction

    }

    public void OnDirectionUnPicked(GameObject dir)
    {
        for (int i = 0; i < _Dir.Length; i++)
        {
            if (_Dir[i] != dir) _Dir[i].gameObject.SetActive(true);
        }

    }
}
