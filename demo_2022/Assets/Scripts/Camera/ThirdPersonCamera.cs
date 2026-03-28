using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;         // 跟随的目标
    public float distance = 5.0f;    // 距离目标的距离
    public float height = 2.0f;      // 摄像机高度
    public float rotationSpeed = 5.0f; // 鼠标旋转速度

    private float currentX = 0f;
    private float currentY = 0f;
    public float minY = -20f;
    public float maxY = 60f;

    void LateUpdate()
    {
        if (target == null) return;

        // 鼠标输入
        currentX += Input.GetAxis("Mouse X") * rotationSpeed; //鼠标左右移动量
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minY, maxY);



        // 计算旋转
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 计算摄像机位置
        Vector3 offset = rotation * new Vector3(0, 0, -distance); //因为在物体的后方，而不是前方
        Vector3 desiredPosition = target.position + Vector3.up * height + offset; //高度偏移+后位偏移

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * height);
    }
}