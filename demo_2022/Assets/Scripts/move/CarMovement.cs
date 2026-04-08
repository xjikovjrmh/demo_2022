using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 8f;
    private Vector3 moveDirection;
    private Rigidbody rb;

    private bool IsAutoForward = false;
    private Coroutine AutoForwardCoroutine;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxisRaw("Vertical");//获取前后移动值

        //if (y != 0)
        //{
        //    move(y);
        //}
        if (Input.GetKeyDown(KeyCode.K))   //两个getKeyDown永远不能同时触发  用一个GetKeyDown ，不是GetKey 
        {
            Debug.Log("开始自动前进");
            //协程的不能传变量y，y不会自动更新
            IsAutoForward = !IsAutoForward;  // 取反：开 

            if (IsAutoForward)
            {
                StartCoroutine(AutoForward());  // 开启自动前进
            }
            else
            {
                StopAllCoroutines();           // 关闭自动前进
            }
        }



    }

    private IEnumerator AutoForward()
    {
        while (true)
        {
            move(1); 
            yield return null; //每帧执行

        }
    }


    private void move(float y)
    {
        moveDirection = transform.forward * y;//注意不要乘time.deltaTime
        //rb.MovePosition(transform.position+moveDirection*speed); //又傻了，不是直接move到这一点，而是在原来基础上累加
        rb.AddForce(moveDirection * speed * 10*1000);//1000是质量，不然移不动

    }
}
