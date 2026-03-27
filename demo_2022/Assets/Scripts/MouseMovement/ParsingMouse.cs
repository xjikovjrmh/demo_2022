using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsingMouse : MonoBehaviour
{
    public Vector2 MouseMovement = Vector2.zero; //存储鼠标移动的向量
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X"); //获取鼠标水平移动的输入
        float mouseY = Input.GetAxis("Mouse Y"); //获取鼠标垂直移动的输入
        float sensitivity = 5f; //旋转灵敏度，可以根据需要调整
        MouseMovement = new Vector2(mouseX, mouseY) * sensitivity;
    }
}

