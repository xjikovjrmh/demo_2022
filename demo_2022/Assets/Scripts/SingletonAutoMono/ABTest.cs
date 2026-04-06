using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        ABMgr.GetInstance().LoadResAsync<GameObject>("model", "tunnel", (obj) => {
            (obj as GameObject).transform.position = -Vector3.up*100;
        });
        ABMgr.GetInstance().LoadResAsync<GameObject>("model", "tunnel", (obj) => {
            (obj as GameObject).transform.position = Vector3.up*100;
        });

    }
}
