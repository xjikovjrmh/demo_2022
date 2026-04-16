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
    //private ModeController modeController;

    public GameObject tram1;
    public GameObject tram2;
    public CarMovement currentTram;
    public GameObject player;

    public KeyCode SwitchTram = KeyCode.B;//切换车号
    public KeyCode SwitchButton = KeyCode.V;
    private int currentCameraIndex = 0;
    private int currentCarNumber = 0;

    

    private void Awake()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        //车初始指向tram1
        currentTram = tram1.GetComponent<CarMovement>();
        tram1.GetComponent<CarMovement>().enabled = false;
        tram2.GetComponent<CarMovement>().enabled = false;

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

        //
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(SwitchButton))
        {
            SwitchToCamera();//别传入参数，否则会因为局部变量而在函数执行后销毁
        }
        //这里要求只在车的视角可以切换车号
        //if (Input.GetKeyDown(SwitchTram)&&currentCameraIndex>0)
        if (Input.GetKeyDown(SwitchTram))
        {
            SwitchToCar();
        }
    }


    //只管理两个车之间的摄像头切换
    private void SwitchToCar()
    {
        currentCarNumber++;
        currentCarNumber %= 2;
        //只要切换相机跟随目标即可
        switch (currentCarNumber)
        {
            case 0:
                Debug.Log("切换到一号车");
                //相机跟随点变化
                //carMovement脚本变化
                currentTram.enabled=false;
                currentTram = tram1.GetComponent<CarMovement>();
                currentTram.enabled = true;
                //camera 脚本变化
                FrontCarCamera.GetComponent<FrontCarCamera>().carHead = tram1FirstCamera;
                ThirdCarCamera.GetComponent<ThirdPersonCamera>().target = tram1FirstCamera; break;
            case 1:
                Debug.Log("切换到二号车");
                currentTram.enabled = false;
                currentTram = tram2.GetComponent<CarMovement>();
                currentTram.enabled = true;

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
                player.GetComponent<PlayerMovement>().enabled = true;
                currentTram.enabled=false;

                //modeController.SwitchMode();
                mainCamera.enabled=true;
                mainCamera.GetComponent<AudioListener>().enabled = true;
                mainCamera.GetComponent<CameraRotation>().enabled = true;

                break;
            case 1:
                //因为是循环的，在这里开始切换Car模式
                //当前指向的车脚本可以移动 ,player禁止
                currentTram.enabled = true;
                player.GetComponent <PlayerMovement>().enabled = false;

                //modeController.SwitchMode();
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
