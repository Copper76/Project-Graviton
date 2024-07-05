using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODPlayerFootsteps : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string inputSound;
    public bool playerismoving;
    public float movementspeed;

    private void Update()
    {
        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
        {
            playerismoving = true;
        }

        else if (Input.GetAxis ("Vertical") == 0 || Input.GetAxis ("Horizontal") == 0)
        {
            playerismoving = false;
        }

//      if 
//      {
//            
//      }




    }

    void CallFootsteps ()
    {
        if (playerismoving == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(inputSound);
        }
    }

    void Start ()
    {
        InvokeRepeating ("CallFootsteps", 0, movementspeed);
    }
    private void OnDisable()
    {
        playerismoving = false;
    }


}