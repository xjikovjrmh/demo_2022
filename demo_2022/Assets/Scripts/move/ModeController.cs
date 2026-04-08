//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ModeController : MonoBehaviour
//{
//    // Start is called before the first frame update
//    private bool IsPeopleMode = true;
//    private bool IsCarMode = false;
//    public GameObject Player;
//    public GameObject Tram1;
//    public GameObject Tram2;
//    private CarMovement carMovement1;
//    private CarMovement carMovement2;
//    private PlayerMovement playerMovement;

//    public CameraController cameraController;

//    void Start()
//    {
//        cameraController = GetComponent<CameraController>();//获取脚本组件用于判断车号

//        InitMode();

//    }
//    public void InitMode()
//    {
//        playerMovement = Player.GetComponent<PlayerMovement>();
//        carMovement1 = Tram1.GetComponent<CarMovement>();
//        carMovement2 = Tram2.GetComponent<CarMovement>();
//        carMovement1.enabled = false;
//        carMovement2.enabled = false;
//        playerMovement.enabled = true;

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.B))
//        {
//            SwitchMode();
//        }
//    }

//    //公共方法 ，在切换视角时顺便调用
//    public void SwitchMode()
//    {
//        //写在函数里面，方便外部调用
//        IsPeopleMode = !IsPeopleMode;
//        IsCarMode = !IsCarMode;
//        Debug.Log("IsPeopleMode:" + IsPeopleMode + " IsCarMode:" + IsCarMode);
//        if (IsPeopleMode && !IsCarMode)
//        {
//            playerMovement.enabled = true;
//            carMovement1.enabled = false;
//            carMovement2.enabled = false;

//        }
//        else if (IsCarMode && !IsPeopleMode)
//        {

//            playerMovement.enabled = false;
//            //这里要判断是哪个车
//            if (cameraController.GetCurrentCarNumber() == 0)
//            {

//                carMovement1.enabled = true;
//                carMovement2.enabled = false;
//            }
//            else
//            {
//                carMovement1.enabled = false;
//                carMovement2.enabled = true;
//            }


//        }
//    }

//}
