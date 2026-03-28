using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform  cameraPosition1;
    public Transform cameraPosition2;
    public Transform cameraPosition3;
    public Camera mainCamera; //这里用GameObject不用camera
    public Camera FrontCarCamera;
    public Camera ThirdCarCamera;

    public KeyCode SwitchButton = KeyCode.V;
    private int currentCameraIndex = 0;


    private void Awake()
    {
        mainCamera.enabled = true;
        FrontCarCamera.enabled = false;
        ThirdCarCamera.enabled = false;
        mainCamera.GetComponent<AudioListener>().enabled = true;
        FrontCarCamera.GetComponent<AudioListener>().enabled = false;
        ThirdCarCamera.GetComponent<AudioListener>().enabled = false;

        mainCamera.GetComponent<CameraRotation>().enabled = true;
        FrontCarCamera.GetComponent<FrontCarCamera>().enabled= false;
        ThirdCarCamera.GetComponent<ThirdPersonCamera>().enabled= false;

    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(SwitchButton))
        {
            SwitchToCamera();//别传入参数，否则会因为局部变量而在函数执行后销毁
        }

        
    }

    private void SwitchToCamera()
    {
        currentCameraIndex++;
        if (currentCameraIndex > 2)
            currentCameraIndex = 0;//循环

        //先禁用所有相机
        mainCamera.enabled=false;
        FrontCarCamera.enabled = false;
        ThirdCarCamera.enabled=false;
        //禁用所有音频组件
        mainCamera.GetComponent<AudioListener>().enabled = false;
        FrontCarCamera.GetComponent<AudioListener>().enabled = false;
        ThirdCarCamera.GetComponent<AudioListener>().enabled = false;

        mainCamera.GetComponent<CameraRotation>().enabled = false;
        FrontCarCamera.GetComponent<FrontCarCamera>().enabled = false;
        ThirdCarCamera.GetComponent<ThirdPersonCamera>().enabled = false;


        switch (currentCameraIndex)
        {
            case 0:
                mainCamera.enabled=true;
                mainCamera.GetComponent<AudioListener>().enabled = true;
                mainCamera.GetComponent<CameraRotation>().enabled = true;

                break;
            case 1:
                FrontCarCamera.enabled=true;
                FrontCarCamera.GetComponent<AudioListener>().enabled = true;
                FrontCarCamera.GetComponent<FrontCarCamera>().enabled=true;

                break;
            case 2:
                ThirdCarCamera.enabled=true;
                ThirdCarCamera.GetComponent<AudioListener>().enabled = true;
                ThirdCarCamera.GetComponent<ThirdPersonCamera>().enabled = true;


                break;

        }

    }
}
