using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//缓存池 管理器
public class PoolMgr : SingletonAutoMono<PoolMgr>
{
    //2.声明柜子(Dictionary) 和抽屉 List Stack Queue
    Dictionary<string ,Stack<GameObject >> poolDic = new Dictionary<string,Stack<GameObject>>();
    private PoolMgr() { }

    //3.拿东西的方法
    public GameObject GetObj(string name)
    {
        GameObject obj;
        //有抽屉且抽屉有东西                 实际为stack
        if (poolDic.ContainsKey(name) && poolDic[name].Count>0)
        {
            obj = poolDic[name].Pop();
            obj.SetActive(true);
        }
        //没抽屉，或抽屉没东西  创造抽屉，或装抽屉
        else
        {
            obj =GameObject.Instantiate(Resources.Load<GameObject>(name));  //加载资源并实例化
            //优化
            obj.name = name; //避免实例化对象在名字后面加（Clone）
        }
       return obj;
    }

    //放东西方法 push
                        //name是抽屉对象的名字


    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);//不是直接移除，而是失活（隐藏）
        //没有就创建
        if(!poolDic.ContainsKey(obj.name))
        {
            poolDic.Add(obj.name, new Stack<GameObject>());
        }
        poolDic[obj.name].Push(obj);

        ////如果存在对应的抽屉容器，直接放
        //if(poolDic.ContainsKey(name))
        //{
        //    poolDic[name].Push(obj);
        //}
        //else  //没有抽屉
        //{
        //    //先创建抽屉，再往抽屉放
        //    poolDic.Add(name, new Stack<GameObject>());
        //    poolDic[name].Push(obj);
        //}
    }
    //清空柜子当中的数据
    public void ClearPool()
    {
        poolDic.Clear();//切场景时，对象都被移除，应该清空柜子，否则内存泄漏，下次取东西会出问题
    }



}
