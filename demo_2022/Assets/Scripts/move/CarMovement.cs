using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 10f;
    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxisRaw("Vertical");//获取前后移动值

        if (y != 0)
        {
            move(y);
        }
    }

    private void move(float y)
    {
        moveDirection = transform.forward * y * Time.deltaTime;
        rb.MovePosition(transform.position+moveDirection*speed); //又傻了，不是直接move到这一点，而是在原来基础上累加
    }
}
