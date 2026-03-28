using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCarCamera : MonoBehaviour
{
    private float mouseSensitivity = 300;//灵敏度
    public Transform car; //第一人称车头位置
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;//正值 向右，上
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        //正式旋转


        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);//摄像机沿双轴旋转
        //
        car.transform.position += car.transform.forward * Time.deltaTime;//朝自己的方向

        transform.position = car.position;

    }

}
