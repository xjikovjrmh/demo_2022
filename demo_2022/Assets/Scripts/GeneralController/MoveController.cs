using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public ModeController modecontroller;
    // Start is called before the first frame update
    void Start()
    {

    }
    float speed = 1.0f;
    // Update is called once per frame
    void Update()
    {
        if(modecontroller.mode == ModeController.Mode.Move)
        {
            // Ctrl 加速
            if(Input.GetKey(KeyCode.LeftControl))
            {
                speed = 2.0f;
            }
            else
            {
                speed = 1.0f;
            }

            Vector3 moveDirection = Vector3.zero;

            // 基于本地坐标系的移动 (跟随相机朝向)
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += transform.forward;
            }
            if(Input.GetKey(KeyCode.S))
            {
                moveDirection -= transform.forward;
            }
            if(Input.GetKey(KeyCode.A))
            {
                moveDirection -= transform.right;
            }
            if(Input.GetKey(KeyCode.D))
            {
                moveDirection += transform.right;
            }

            // 基于世界坐标系的垂直移动
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection += Vector3.up;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection += Vector3.down;
            }

            // 应用位置更改，这里修复了原本 this.transform += 的语法错误。
            // 使用 moveDirection.normalized 确保斜向移动时速度不会叠加变快。
            if (moveDirection != Vector3.zero)
            {
                transform.position += moveDirection.normalized * (Time.deltaTime * 10f * speed);
            }
        }
    }
}
