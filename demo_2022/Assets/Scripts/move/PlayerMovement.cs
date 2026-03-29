using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private float speed = 5f;
    private float airspeed = 7f;
    public Vector3 moveDirection;

    //鼠标速度 
    public float mouseSensitivity = 300f;
    private float yRotation;//只记录y轴

    private KeyCode Up = KeyCode.Space;
    private KeyCode Down = KeyCode.LeftControl;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // 隐藏并锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float y = Input.GetAxisRaw("Vertical");//垂直
        float x = Input.GetAxisRaw("Horizontal");//水平
        if(x!=0||y!=0)  //不是大于0
        {
            Move(x, y);

        }
        if(Input.GetKey(Up)) //持续发力要getKey
        {
            Debug.Log("Up");
            fly();
        }
        if (Input.GetKey(Down))
        {
            Debug.Log("Down");
            ground();
        }


    }
    private void ground()
    {
        rb.MovePosition(transform.position-transform.up*airspeed*Time.deltaTime);
    }

    private void fly()
    {
        rb.MovePosition(transform.position + transform.up *airspeed * Time.deltaTime);
    }

    private void Update()
    {
        //鼠标控制角色旋转
        RotateWithMouse();

        //

    }

    private void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX;//累加 不然每次只旋转（实际上是移动到）一点
        transform.rotation = Quaternion.Euler(0, yRotation, 0);//只绕y旋转
    }

    private void Move(float x,float y)
    {
        //移动过程中旋转后需要改变方向
        //这是固定世界坐标前方的移动 
        //Vector3 Direction = new Vector3(x,y,0);
        //Direction.Normalize();
        //rb.MovePosition(transform.position+Direction*speed*Time.deltaTime);
        
        moveDirection = (transform.right*x+transform.forward*y).normalized;
        rb.MovePosition(transform.position+moveDirection *speed* Time.deltaTime);

    }
}
