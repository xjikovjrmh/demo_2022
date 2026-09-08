using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    //存储两个tram的两个机位,
    //用数组简化
    private CarMovement[] tramMovements = new CarMovement[2];//0表示一号车，1表示二号车
    [SerializeField] private Transform[] firstCameraPoints;
    [SerializeField] private Transform[] thirdCameraPoints;

    //相机
    public Camera mainCamera; //这里用GameObject不用camera
    public Camera FrontCarCamera;
    public Camera ThirdCarCamera;
    //private ModeController modeController;
    //车人
    public GameObject tram1;
    public GameObject tram2;
    public CarMovement currentTram;
    public GameObject player;

    public KeyCode SwitchTram = KeyCode.B;//切换车号
    public KeyCode SwitchButton = KeyCode.V;
    private int currentCameraIndex = 0;
    private int currentCarNumber = 0;
    //脚本
    private PlayerMovement playerMovement;
    private CameraRotation cameraRotation;
    private FrontCarCamera frontCarCamera;
    private ThirdPersonCamera thirdPersonCamera;
    

    private void Awake()
    {   
        
        //赋值给脚本
        InitializeScripts();
        player.GetComponent<PlayerMovement>().enabled = true;
        //初始化  不是从inspecto中拖入的数组必须先 new ，否则会报空引用错误
        tramMovements[0]= tram1.GetComponent<CarMovement>();
        tramMovements[1] = tram2.GetComponent<CarMovement>();
        //车初始指向tram1
        currentTram = tramMovements[currentCarNumber];
        
        tramMovements[0].enabled = true;
        tramMovements[1].enabled = false;
        
        //开始相机只保留主相机
        mainCamera.enabled = true;
        FrontCarCamera.enabled = false;
        ThirdCarCamera.enabled = false;
        //只保留主相机的AudioListener脚本
        
        //只激活主相机跟随脚本
        cameraRotation.enabled = true;
        frontCarCamera.enabled= false;
        thirdPersonCamera.enabled= false;

        //
    }
    private void InitializeScripts()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraRotation = mainCamera.GetComponent<CameraRotation>();
        frontCarCamera = FrontCarCamera.GetComponent<FrontCarCamera>();
        thirdPersonCamera = ThirdCarCamera.GetComponent<ThirdPersonCamera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(SwitchButton))//V
        {
            SwitchToCamera();//别传入参数，否则会因为局部变量而在函数执行后销毁
        }
        //这里要求只在车的视角可以切换车号
        //if (Input.GetKeyDown(SwitchTram)&&currentCameraIndex>0)
        if (Input.GetKeyDown(SwitchTram)) //B
        {
            SwitchToCar();
        }
    }


    //只管理两个车之间的摄像头切换
    private void SwitchToCar()
    {
        currentCarNumber++;
        currentCarNumber %= 2;
        Debug.Log("切换到" + (currentCarNumber + 1) + "号车");
        //相机跟随点变化
        //carMovement脚本变化
        currentTram.enabled=false;
        currentTram = tramMovements[currentCarNumber];
        currentTram.enabled = true;
        //camera 脚本属性赋值
        frontCarCamera.carHead = firstCameraPoints[currentCarNumber];
        thirdPersonCamera.target = thirdCameraPoints[currentCarNumber]; 
           
    }
    private void SwitchToCamera()
    {

        Debug.Log("切换到" + (currentCameraIndex + 1) + "号相机");
        currentCameraIndex++;
        currentCameraIndex %= 3;

        //先禁用所有相机
        DisableAllCamera();
        //脚本
        cameraRotation.enabled = false;
        frontCarCamera.enabled = false;
        thirdPersonCamera.enabled = false;

        switch (currentCameraIndex)
        {
            case 0:
                //car模式结束
                player.GetComponent<PlayerMovement>().enabled = true;
                currentTram.enabled=false;

                //modeController.SwitchMode();
                //两行都要写， 第一行是禁用相机渲染，其脚本还在跑
                mainCamera.enabled=true;
                
                cameraRotation.enabled = true;

                break;
            case 1:
                //因为是循环的，在这里开始切换Car模式
                //当前指向的车脚本可以移动 ,player禁止
                currentTram.enabled = true;
                player.GetComponent <PlayerMovement>().enabled = false;

                //modeController.SwitchMode();
                FrontCarCamera.enabled=true;
                
                frontCarCamera.enabled=true;

                break;
            case 2:
                ThirdCarCamera.enabled=true;
                
                thirdPersonCamera.enabled = true;
                break;
        }

    }
    private void DisableAllCamera()
    {
        mainCamera.enabled=false;
        FrontCarCamera.enabled = false;
        ThirdCarCamera.enabled=false;
    }
}
