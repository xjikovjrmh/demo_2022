using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAction : MonoBehaviour
{
    // Start is called before the first frame update

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EventCenter.GetInstance().Publish(EventCenter.GameEvent.MonsterDeathEvent);
        }
    }

}
