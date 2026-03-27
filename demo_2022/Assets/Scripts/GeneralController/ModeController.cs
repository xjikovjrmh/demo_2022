using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Mode
    {
        Move, Control
    };
    public Mode mode = Mode.Move;
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            mode = Mode.Control;
        }
        else
        {
            mode = Mode.Move;
        }
    }
}
