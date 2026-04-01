using System.Collections;
using System.Collections.Generic;
using UnityEngine;

                     // 泛型                    泛型约束
public class SingletonAutoMono<T> : MonoBehaviour where T :MonoBehaviour
{                //只有继承自MonoBehaviour基类的类才能传进来

    private static T instance;
    public static T GetInstance()
    {
        if(instance ==null)
        {
            GameObject obj = new GameObject();
            // 将对象改名为脚本名 ，方便编辑器 看到改对象
            obj.name = typeof(T).ToString();
            //过场景时不销毁
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<T>();
        }
        return instance;
    }
}
