using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsPeopleMode =true;
    private bool IsCarMode = false;
    public GameObject Player;
    public GameObject Car;
    private CarMovement carMovement;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement =Player.GetComponent<PlayerMovement>();
        carMovement =Car.GetComponent<CarMovement>();
        carMovement.enabled = false;
        playerMovement.enabled = true;


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            IsPeopleMode = !IsPeopleMode;
            IsCarMode = !IsCarMode;
            Debug.Log("IsPeopleMode:"+IsPeopleMode+" IsCarMode:" + IsCarMode);

            SwitchMode();
        }
        
    }

    private void SwitchMode()
    {
        if (IsPeopleMode && !IsCarMode)
        {
            playerMovement.enabled = true;
            carMovement.enabled = false;

        }
        else if (IsCarMode && !IsPeopleMode)
        {
            {
                playerMovement.enabled= false;
                carMovement.enabled = true;
            }
        }
    }
    
}
