using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable] class TestData
    {
        public bool testBool;

    }

    void Start()
    {
        var testData = new TestData();
        Debug.Log(JsonUtility.ToJson(testData));    //打印的是空的，这个字段没有任何值，且testbool是原始类型
        //需要把字段写进类中，用Serializable序列化，初始化对象后才能用JsonUtility序列化
        //该方法使用 Unity 串行器; 因此，你传递的对象必须由序列化器支持：
        //它必须是 MonoBehaviour、ScriptableObject 或应用 Serializable 属性的纯类/ 结构体
        //公有的普通字段 public string publicField    带有Serializable （三种属性任意）属性的字段 才能被序列化
    }

}
