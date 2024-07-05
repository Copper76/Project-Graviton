using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private AnimationCurve offsetCurve;

    private FMOD.Studio.EventInstance DoorOpeningSound;

    private Coroutine _stopCoroutine;

    private const float Epsilon = 1.0e-6f;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private Vector3 _currentStartPosition;
    private Vector3 _targetPosition;
    
    private bool _isOpen;
    private bool _doorMoving;
    private float _animationTime;
    
    private RelativeTime _relativeTime;
    private bool _useRelativeTime;

    

    private void Start()
    {
        DoorOpeningSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/GeneralEnvironment/DoorOpening");
        DoorOpeningSound.setParameterByName("DoorFullClosed", 20f);
        


        _closedPosition = transform.position;
        _openPosition = _closedPosition + offSet;

        _isOpen = false;
        _targetPosition = _closedPosition;
        _currentStartPosition = _closedPosition;

        _useRelativeTime = false;
        
        if (TryGetComponent(out RelativeTime relativeTime))
        {
            _relativeTime = relativeTime;
            _useRelativeTime = true;
    //        DoorOpeningSound.setParameterByName("TimeDilation", _relativeTime.GetTimeMultiplier());
        }

        else
        {
    //        DoorOpeningSound.setParameterByName("TimeDilation", 0f);
        }

        
    }

    public void ToggleDoor()
    {

        DoorOpeningSound.start();
        DoorOpeningSound.setParameterByName("DoorFullClosed", 20f);

        if (_stopCoroutine != null)
        {
            StopCoroutine(_stopCoroutine);
        }

        _isOpen = !_isOpen;
        _doorMoving = true;
        _currentStartPosition = transform.position;
        _targetPosition = _isOpen ? _openPosition : _closedPosition;
        _animationTime = 0f;
        

    }

    private void Update()
    {

        DoorOpeningSound.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
   //     DoorOpeningSound.setParameterByName("TimeDilation", _relativeTime.GetTimeMultiplier());


        if (Vector3.Distance(transform.position, _targetPosition) < Epsilon && _doorMoving) 
        {
           DoorOpeningSound.setParameterByName("DoorFullClosed", 80f);

            _stopCoroutine = StartCoroutine(StopAudio());
            _doorMoving = false;

            return;

        }

        float deltaTime = _useRelativeTime ? _relativeTime.DeltaTime() : Time.deltaTime;
        _animationTime += deltaTime * smoothSpeed;
        float curveValue = offsetCurve.Evaluate(_animationTime);
        transform.position = Vector3.Lerp(_currentStartPosition, _targetPosition, curveValue);
    }


    private IEnumerator StopAudio()
    {
        yield return new WaitForSeconds(3);

            DoorOpeningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
  
    }
}