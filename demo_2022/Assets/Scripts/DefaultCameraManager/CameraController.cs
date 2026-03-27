using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [Header("Dependencies")]
    public ParsingMouse parsingMouse; // 引用ParsingMouse组件

    [Header("Camera Constraints")]
    [Tooltip("上下能看的最大角度，90表示能正对头顶与脚底")]
    public float verticalClamp = 90f;

    // 用来累计摄像机的X轴旋转角度（俯视/仰视）
    private float xRotation = 0f;

    public enum RotationMode
    {
        Local,
        Roundabout
    }
    public RotationMode rotationMode = RotationMode.Local;

    void OnEnable()
    {
        // 每次激活时自动从当前Transform读取X轴旋转并换算为-180~180的角度，防止由于切换造成的跳劈(Snap)
        xRotation = transform.localEulerAngles.x;
        if (xRotation > 180f) 
        {
            xRotation -= 360f;
        }
    }

    void Start()
    {
        // 可选：为了测试方便，先锁定且隐藏鼠标指针（常用于第一人称）
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void Update()
    {
        if (parsingMouse == null) return;

        Vector2 mouseInput = parsingMouse.MouseMovement; // 获取鼠标移动增量

        // 鼠标水平移动 -> 控制左右旋转 (绕Y轴)
        float horizontalRotation = mouseInput.x;

        // 鼠标垂直移动 -> 控制上下旋转 (绕X轴)
        // 注意：这里使用减去 mouseInput.y，因为鼠标向上推的时候，摄像基的X轴角度需要变小才能抬头
        xRotation -= mouseInput.y;

        // 核心逻辑：使用 Clamp 限制X轴旋转角度在 [-verticalClamp, verticalClamp] 之间
        // 当设置为90时，向上最多转到-90度（正头顶），向下最多转到90度（脚底）
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        // 1. 将上面计算并限制过后的上下旋转值，单独应用到这个摄像机的本地旋转上
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f);

        if (rotationMode == RotationMode.Local)
        {
            // 第一人称 (Local)
            // 将左右旋转增量应用到 Y 轴（这里我们基于世界坐标系的Y轴转，防止斜着转）
            transform.Rotate(Vector3.up * horizontalRotation, Space.World);
        }
        else if (rotationMode == RotationMode.Roundabout)
        {
            // 第三人称 (Roundabout: 围绕父对象旋转)
            // 子对象获取父对象的坐标就是使用：transform.parent.position
            if (transform.parent != null)
            {
                transform.RotateAround(transform.parent.position, Vector3.up, horizontalRotation);
            }
            else
            {
                // 如果没有挂载在任何父对象下，暂时绕世界坐标原点旋转
                transform.RotateAround(Vector3.zero, Vector3.up, horizontalRotation);
            }
        }
    }
}
