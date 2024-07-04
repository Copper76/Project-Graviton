using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Control 6 arrows in select/unselect/reposition.
/// </summary>
public class SelectedUI : MonoBehaviour
{
    [SerializeField] private Transform[] dirTransform;//in order same as enum Direction
    [SerializeField] private float highlightIntensity;

    private Vector3[] _oriDirPos;
    private Material[] _materials;

    private Dictionary<Vector3, int> _dirIndexByVector;

    private void Awake()
    {
        _oriDirPos = new Vector3[dirTransform.Length];
        _materials = new Material[dirTransform.Length];
        //Store original arrows position for the use of ResetPos()
        for (int i = 0; i < _oriDirPos.Length; i++)
        {
            _oriDirPos[i] = dirTransform[i].localPosition;
            _materials[i] = dirTransform[i].GetComponent<MeshRenderer>().material;
        }

        _dirIndexByVector = new Dictionary<Vector3, int>
        {
            { Vector3.right, 0},
            { Vector3.left, 1},
            { Vector3.up, 2},
            { Vector3.down, 3},
            { Vector3.forward, 4},
            { Vector3.back, 5},
        };
    }

    public void UpdateArrowVisual(Vector3 gravityDir)
    {
        UpdateArrowVisual(_dirIndexByVector[gravityDir]);
    }


    private void UpdateArrowVisual(int dirIndex)
    {
        for (int i = 0; i < dirTransform.Length; i++)
        {
            if (dirIndex != i)
            {
                UnHighlightArrow(i);
                GhostArrow(i);
            }
            else
            {
                HighlightArrow(i);
                ResetGhostArrow(i);
            }
        }
    }

    private void HighlightArrow(int selectedDirectionIndex)
    {
        _materials[selectedDirectionIndex].SetFloat("_HighlightIntensity", highlightIntensity);
    }

    private void UnHighlightArrow(int selectedDirectionIndex)
    {
        _materials[selectedDirectionIndex].SetFloat("_HighlightIntensity", 0.0f);
    }

    private void GhostArrow(int index)
    {
        _materials[index].SetFloat("_Alpha", 0.3f);
    }

    private void ResetGhostArrow(int index)
    {
        _materials[index].SetFloat("_Alpha", 1.0f);
    }

    /// <summary>
    /// Reset arrows position on selected object.
    /// To avoid the last time offsets from the resizeUI(), it works only to the origin arrow condition.
    /// </summary>
    public void ResetPos()
    {
        //Reset the position first in case it has been resize
        for (int i = 0; i < _oriDirPos.Length; i++)
        {
            dirTransform[i].localPosition = _oriDirPos[i];
            dirTransform[i].localScale = Vector3.one;
        }
    }

    /// <summary>
    /// Resize individual arrows by target object's bound info
    /// </summary>
    /// <param name="_boundInfo"> Distance from center to bound on xyz axis.</param>
    public void ResizeUI(float[] _boundInfo)
    {
        ResetPos();

        Vector3 _newDir;
        float _offset;
        for (int i = 0; i < _boundInfo.Length; i++)
        {

            //Condition the boundDistance < 1, resize arrow instead
            if (Mathf.Abs(_boundInfo[i]) < 0.5f)
            {
                dirTransform[i].localScale = Vector3.one * Mathf.Abs(_boundInfo[i]) / 0.5f;
            }

            else
            {
                _newDir = dirTransform[i].position;

                _offset = (i % 2 != 0) ? _boundInfo[i] + 0.5f : _boundInfo[i] - 0.5f;


                if (i < 2)//x
                    _newDir.x += _offset;
                else if (i < 4)//y
                    _newDir.y += _offset;
                else//z
                    _newDir.z += _offset;

                dirTransform[i].position = _newDir;
            }
        }
    }
}
