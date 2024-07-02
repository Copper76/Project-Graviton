using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectedUI : MonoBehaviour
{
    private enum Direction
    {
        PosX,
        NegX,
        PosY,
        NegY,
        PosZ,
        NegZ,
    }
    private int _selectedDirection;
    private GameObject _currDirObject;

    [SerializeField] private Transform[] _DirTransform;//in order same as enum Direction,


    

    private Vector3[] _OriDirPos;

    private void Awake()
    {
        _OriDirPos = new Vector3[_DirTransform.Length];
        for (int i = 0; i < _OriDirPos.Length; i++)
        {
            _OriDirPos[i] = _DirTransform[i].localPosition;
        }
    }
    private void Start()
    {
        
        
    }

    //Test
    public void TestDirection()
    {
        //TEST
        StartCoroutine(testIt());
    }
    //Test
    private IEnumerator testIt()
    {
        for (int i = 0; i < _DirTransform.Length; i++)
        {
            yield return new WaitForSeconds(2f);
            OnDirectionPicked(_DirTransform[i].gameObject);

            yield return new WaitForSeconds(2f);
            OnDirectionUnPicked();
        }
        

    }
    /// <summary>
    /// Invisible other gizmo direction UI
    /// </summary>
    public void OnDirectionPicked(GameObject dirObject)
    {
        _currDirObject = dirObject;

        //set unselect axis invisible
        for (int i = 0; i < _DirTransform.Length; i++)
        {

            if (_DirTransform[i].gameObject != dirObject)
            {
                _DirTransform[i].gameObject.SetActive(false);
            }

            else 
            { 
                _selectedDirection = i; 
            }
            
        }

        //TODO: move correspondingly by player interaction

    }

    public void OnDirectionUnPicked()
    {
        //Set all the axis visible
        for (int i = 0; i < _DirTransform.Length; i++)
        {
            if (_DirTransform[i] != _currDirObject) _DirTransform[i].gameObject.SetActive(true);
        }

    }

    public void ResetPos()
    {
        //Reset the position first in case it has been resize
        for (int i = 0; i < _OriDirPos.Length; i++)
        {
            _DirTransform[i].localPosition = _OriDirPos[i];
        }
    }
    public void ResizeUI(float[] _boundInfo)
    {
        ResetPos();

        Vector3 _newDir;
        float _offset;
        for (int i = 0; i < _boundInfo.Length; i++)
        {
            _newDir = _DirTransform[i].position;

            if (i % 2 != 0) _offset = -_boundInfo[i] - 0.5f;
            else _offset = _boundInfo[i] - 0.5f;
            
            if (i % 2 != 0)  _offset = -_offset;


            if (i<2)//x
                _newDir.x += _offset;
            else if (i<4)//y
                _newDir.y += _offset;
            else//z
                _newDir.z += _offset;

            _DirTransform[i].position = _newDir;
            
        }
    }
}
