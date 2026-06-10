using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private float mouseSensitivity = 300;
    public Transform playerBody;
    public float xRotation = 0f;
    public float yRotation = 0f;

    private bool inputReady = false;
    private bool hasInitialized = false;


    // 获取当前状态（用于保存）
    public Vector2 GetRotationState()
    {
        return new Vector2(xRotation, yRotation);
    }

    // 设置状态（用于加载）
    public void SetRotationState(Vector2 state)
    {
        xRotation = state.x;
        yRotation = state.y;

        // 立即应用
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerBody.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //将相机的初始角度与Player 保持一致
       Vector3 currentEuler = transform.rotation.eulerAngles;
        xRotation = currentEuler.x; //因为后面的mouse y ，mousex是偏移量，累加到这两个上面，不会清零
        yRotation = currentEuler.y;
    }

    private void Update()
    {

        //前两帧专门用来"吸收"加载期间累积的鼠标位移
        if (!hasInitialized)
        {
            if (!inputReady)
            {
                // 第一帧：什么都不做，只消耗掉累积的输入
                Input.GetAxis("Mouse X");
                Input.GetAxis("Mouse Y");
                inputReady = true;
                return;
            }
            else
            {
                // 第二帧：再消耗一次残余，然后标记初始化完成
                Input.GetAxis("Mouse X");
                Input.GetAxis("Mouse Y");
                hasInitialized = true;
                return;
            }
        }
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if (mouseX != 0 || mouseY != 0)
        {
            yRotation += mouseX * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY * mouseSensitivity * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerBody.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        transform.position = playerBody.position;
    }
}