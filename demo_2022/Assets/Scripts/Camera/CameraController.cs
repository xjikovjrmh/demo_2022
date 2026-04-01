using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    //存储两个tram的两个机位,
    public Transform tram1FirstCamera;
    public Transform tram1ThirdCamera;
    public Transform tram2FirstCamera;
    public Transform tram2ThirdCamera;

    public Camera mainCamera; //这里用GameObject不用camera
    public Camera FrontCarCamera;
    public Camera ThirdCarCamera;
    private ModeController modeController;
    
    public KeyCode SwitchTram = KeyCode.B;//切换车号
    public KeyCode SwitchButton = KeyCode.V;
    private int currentCameraIndex = 0;
    private int currentCarNumber = 0;



    private void Awake()
    {
        modeController = GetComponent<ModeController>();

        //调用modeController的初始化方法
        modeController.InitMode();
        //开始相机只保留主相机
        mainCamera.enabled = true;
        FrontCarCamera.enabled = false;
        ThirdCarCamera.enabled = false;
        //只保留主相机的AudioListener脚本
        mainCamera.GetComponent<AudioListener>().enabled = true;
        FrontCarCamera.GetComponent<AudioListener>().enabled = false;
        ThirdCarCamera.GetComponent<AudioListener>().enabled = false;
        //只激活主相机跟随脚本
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

        if (Input.GetKeyDown(SwitchTram))
        {
            SwitchToCar();
        }
    }

    private void SwitchToCar()
    {
        currentCarNumber++;
        currentCarNumber %= 2;
        //只要切换相机跟随目标即可
        switch (currentCarNumber)
        {
            case 0:
                Debug.Log("切换到一号车");
                FrontCarCamera.GetComponent<FrontCarCamera>().carHead = tram1FirstCamera;
                ThirdCarCamera.GetComponent<ThirdPersonCamera>().target = tram1FirstCamera; break;
            case 1:
                Debug.Log("切换到二号车");
                FrontCarCamera.GetComponent<FrontCarCamera>().carHead = tram2FirstCamera;
                ThirdCarCamera.GetComponent<ThirdPersonCamera>().target = tram2FirstCamera; break;

        }

        
    }
    private void SwitchToCamera()
    {
        currentCameraIndex++;
        currentCameraIndex %= 3;

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
                //car模式结束
                modeController.SwitchMode();
                mainCamera.enabled=true;
                mainCamera.GetComponent<AudioListener>().enabled = true;
                mainCamera.GetComponent<CameraRotation>().enabled = true;

                break;
            case 1:
                //因为是循环的，在这里开始切换Car模式
                modeController.SwitchMode();
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
