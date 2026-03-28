using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private float mouseSensitivity = 300;//灵敏度
    public Transform playerBody; //第一人称玩家位置
    public float xRotation = 0f;//俯仰角
    public float yRotation = 0f;
    private void Start()
    {
        //光标锁屏幕中央并且不可见
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() //FixedUpdate 太慢
    {
        //mouseX 是鼠标绕X轴旋转的值，控制yRotation角
        float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;//鼠标左右移动值
        float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;
        yRotation += mouseX;  
        xRotation -= mouseY;
        xRotation =Mathf.Clamp(xRotation, -90f, 90f);//上下旋转
        //yRotation =Mathf.Clamp(yRotation, -90f, 90f);// 左右旋转 取消范围，可以无限旋转
        //正式旋转
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);//摄像机沿双轴旋转
        //这是角色的选择控制脚本
        playerBody.rotation = Quaternion.Euler(0, yRotation, 0);//角色只沿y轴旋转 //不要传入xRotation 会歪

        //相机跟随
        transform.position=playerBody.position;
        
    }


}
