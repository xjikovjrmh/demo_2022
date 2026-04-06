using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayRemove : MonoBehaviour
{
    // Start is called before the first frame update

    //public string poolName;
    private void OnEnable()//每次激活时执行，而不是start，
    {
        //可以延迟执行
        Invoke("RemoveMe", 1f);
    }

    // Update is called once per frame

    private void RemoveMe()
    {
        PoolMgr.GetInstance().PushObj(this.gameObject);
    }
}
