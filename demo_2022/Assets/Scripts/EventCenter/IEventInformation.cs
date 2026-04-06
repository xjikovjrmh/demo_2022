using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInformation { }
public class EventInfo<T> : IEventInformation 
{
    // 带参数的委托，必须装有泛型参数的方法
    public UnityAction<T> actions;
    //构造函数
    public EventInfo(UnityAction<T> actions)
    {
        this.actions += actions;
    }

}
public class EventInfo : IEventInformation
{
    //装无参的方法
    public UnityAction actions;
    public EventInfo(UnityAction actions)
    {
        this.actions += actions;
    }

}