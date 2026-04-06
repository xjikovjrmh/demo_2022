using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAction : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        EventCenter.GetInstance().Subscribe(EventCenter.GameEvent.MonsterDeathEvent, OnTaskUpdate);
    }


    public void OnTaskUpdate()
    {
        Debug.Log("任务更新");
    }

}
