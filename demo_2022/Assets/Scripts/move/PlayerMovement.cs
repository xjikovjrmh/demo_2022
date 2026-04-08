using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private float speed = 8f;
    private float airspeed = 12f;
    private float speedUp = 5f;
    private bool IsSpeed = false;
    public Vector3 moveDirection;
    //private float highspeed = 10f;
    public float F=10;

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
    private void Update()
    {
        //鼠标控制角色旋转
        RotateWithMouse();
        if (Input.GetKeyDown(KeyCode.LeftShift))//这个必须放在update里面，否则丢失
        {
            Debug.Log("加速");
            IsSpeed = !IsSpeed;
        }
        

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        float currentSpeed = IsSpeed?speed*speedUp:speed;
        float currentAirSpeed = IsSpeed?airspeed * speedUp : airspeed;
        // 按住左Shift → 切换模式


        float y = Input.GetAxisRaw("Vertical");//垂直
        float x = Input.GetAxisRaw("Horizontal");//水平
        if(x!=0||y!=0)  //不是大于0
        {
            Move(x, y,currentSpeed);

        }
        if(Input.GetKey(Up)) //持续发力要getKey
        {
            Debug.Log("Up");
            fly(currentAirSpeed);
        }
        if (Input.GetKey(Down))
        {
            Debug.Log("Down");
            ground(currentAirSpeed);
        }

    }
    //不会无限加速，固定在currentSpeed*F/drag
    private void ground(float currentspeed)
    {
        rb.AddForce(-transform.up *F* currentspeed, ForceMode.Acceleration);
        //rb.MovePosition(transform.position-transform.up* currentspeed * Time.deltaTime);
    }

    private void fly(float currentspeed)
    {
        rb.AddForce(transform.up*F* currentspeed, ForceMode.Acceleration);

        //rb.MovePosition(transform.position + transform.up * currentspeed * Time.deltaTime);
    }



    private void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX;//累加 不然每次只旋转（实际上是移动到）一点
        transform.rotation = Quaternion.Euler(0, yRotation, 0);//只绕y旋转
    }

    private void Move(float x,float y,float currentspeed)
    {
        //移动过程中旋转后需要改变方向
        //这是固定世界坐标前方的移动 
        //Vector3 Direction = new Vector3(x,y,0);
        //Direction.Normalize();
        //rb.MovePosition(transform.position+Direction*speed*Time.deltaTime);

        // ============== 核心：判断是否按Shift加速 ==============
        
        moveDirection = (transform.right*x+transform.forward*y).normalized;

        rb.AddForce(moveDirection *F* currentspeed, ForceMode.Acceleration);

    }
}
