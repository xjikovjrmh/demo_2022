using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : SingletonAutoMono<EventCenter>
{
    //使用枚举，(新的数据类型)在其他脚本可以便捷查询有哪些事件
    //委托可以装任意访问权限的函数，
    public enum GameEvent
    {
        MonsterDeathEvent  //变量相当于0

    }
    private Dictionary<GameEvent, IEventInformation> eventDic =new Dictionary<GameEvent, IEventInformation>();
    public void Subscribe<T>(GameEvent eventName,UnityAction<T> action)
    {
        if(!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
        //向下转型,  从字典取出的是父接口IEventInformation ，要转为对应子类型
        (eventDic[eventName] as EventInfo<T>).actions += action;
    }
    public void Subscribe(GameEvent eventName, UnityAction action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(eventName, new EventInfo(action));
        }
    }
    public void UnSubscribe(GameEvent eventName, UnityAction action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions -= action;
        }
        
    }
    public void UnSubscribe<T>(GameEvent eventName, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= action;
        }
    }
    //事件触发
    public void Publish(GameEvent eventName)
    {
        if(eventDic.ContainsKey(eventName)) 
            {//执行前会判断actions是否为空，若为空则不调用
            ((eventDic[eventName]) as EventInfo).actions?.Invoke();
            }
    }
    public void Publish<T>(GameEvent eventName,T data)
    {
        if (eventDic.ContainsKey(eventName))
        {//执行前会判断actions是否为空，若为空则不调用
            ((eventDic[eventName]) as EventInfo<T>).actions?.Invoke(data);
        }
    }
    public void ClearAllEvent()
    {
        eventDic.Clear();
    }



}
