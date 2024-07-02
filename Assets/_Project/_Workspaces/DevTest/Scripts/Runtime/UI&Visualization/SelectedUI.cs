using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;





/// <summary>
/// Control 6 arrows in select/unselect/reposition.
/// </summary>
public class SelectedUI : MonoBehaviour
{

    //Not been used for now
    private enum Direction
    {
        PosX,
        NegX,
        PosY,
        NegY,
        PosZ,
        NegZ,
    }


    private int _selectedDirectionIndex;
    private GameObject _currDirObject;
    private Vector3 _movingDirection;
    private GameObject _currentTarget;
    private float _movingSpeed;
    private Vector3 _targetPos;

    [SerializeField] private Transform[] _DirTransform;//in order same as enum Direction

    private Vector3[] _OriDirPos;

    private void Awake()
    {
        _OriDirPos = new Vector3[_DirTransform.Length];
        _movingDirection = Vector3.zero;
        //Store original arrows position for the use of ResetPos()
        for (int i = 0; i < _OriDirPos.Length; i++)
        {
            _OriDirPos[i] = _DirTransform[i].localPosition;
        }
    }

    private void Update()
    {
        if (_movingDirection == Vector3.zero) return;
        MovingTarget();
    }

    private void MovingTarget()
    {
        _targetPos = _currentTarget.transform.position;
        _targetPos += _movingDirection * _movingSpeed * Time.deltaTime;
        _currentTarget.transform.position = _targetPos;
    }
    ////////////////////////////////////////////////////////////////////
    //Test
    public void TestMovingDirection(GameObject _target, float movingSpeed)
    {
        //TEST
        StartCoroutine(testIt(_target, movingSpeed));
        
    }
    //Test if pick, unpick, resizing is working
    private IEnumerator testIt(GameObject _target, float movingSpeed)
    {
        for (int i = 0; i < _DirTransform.Length; i++)
        {
            yield return new WaitForSeconds(2f);
            OnDirectionPicked(_DirTransform[i].gameObject, _target, true, movingSpeed);
            
            yield return new WaitForSeconds(2f);
            OnDirectionUnPicked();
        }
    }
    ////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Invisible other gizmo direction UI
    /// </summary>
    public void OnDirectionPicked(GameObject _dirObject,GameObject _target, bool _sameDir, float movingSpeed)
    {
        _currentTarget = _target;
        _currDirObject = _dirObject;
        _movingSpeed = movingSpeed;

        //set unselect axis invisible
        for (int i = 0; i < _DirTransform.Length; i++)
        {

            if (_DirTransform[i].gameObject != _dirObject)
            {
                _DirTransform[i].gameObject.SetActive(false);
            }

            else
            {
                _selectedDirectionIndex = i;
            }

        }

        //TODO: move correspondingly by player interaction
        if (!_sameDir) _selectedDirectionIndex += 1;

        switch ((Direction)_selectedDirectionIndex)
        {
            case Direction.PosX:
                _movingDirection = Vector3.right;
                break;
            case Direction.NegX:
                _movingDirection = Vector3.left;
                break;
            case Direction.PosY:
                _movingDirection = Vector3.up;
                break;
            case Direction.NegY:
                _movingDirection = Vector3.down;
                break;
            case Direction.PosZ:
                _movingDirection = Vector3.forward;
                break;
            case Direction.NegZ:
                _movingDirection = Vector3.back;
                break;
        }



    }

    public void OnDirectionUnPicked()
    {
        _movingDirection = Vector3.zero;
        _currentTarget = null;
        _currDirObject = null;

        //Set all the axis visible
        for (int i = 0; i < _DirTransform.Length; i++)
        {
            if (_DirTransform[i] != _currDirObject) _DirTransform[i].gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// Reset arrows position on selected object.
    /// To avoid the last time offsets from the resizeUI(), it works only to the origin arrow condition.
    /// </summary>
    public void ResetPos()
    {
        //Reset the position first in case it has been resize
        for (int i = 0; i < _OriDirPos.Length; i++)
        {
            _DirTransform[i].localPosition = _OriDirPos[i];
            _DirTransform[i].localScale = Vector3.one;
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
                _DirTransform[i].localScale *= Mathf.Abs(_boundInfo[i]) / 0.5f;
            }

            else
            {
                _newDir = _DirTransform[i].position;

                _offset = (i % 2 != 0) ? _boundInfo[i] + 0.5f : _offset = _boundInfo[i] - 0.5f;


                if (i < 2)//x
                    _newDir.x += _offset;
                else if (i < 4)//y
                    _newDir.y += _offset;
                else//z
                    _newDir.z += _offset;


                _DirTransform[i].position = _newDir;
            }
            

        }
    }
}
