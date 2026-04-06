using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        EventCenter.GetInstance().Subscribe(EventCenter.GameEvent.MonsterDeathEvent, OnPlayerGetAward);
    }

    //
    private void OnPlayerGetAward()
    {
        Debug.Log("玩家获得奖励");
    }
}
