using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public ModeController modecontroller;
    public GameObject frontCamera;
    public GameObject thirdPartyCamera;
    void Start()
    {
        if (frontCamera != null && thirdPartyCamera != null)
        {
            frontCamera.SetActive(true);
            thirdPartyCamera.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(modecontroller.mode);
        if(modecontroller.mode == ModeController.Mode.Control)
        {
            if(Input.GetKeyDown("c"))//C键切换摄像头
            {
                if(frontCamera != null && thirdPartyCamera != null)
                {
                    bool isFront = frontCamera.activeSelf;
                    frontCamera.SetActive(!isFront);
                    thirdPartyCamera.SetActive(isFront);
                }
            }
        }
    }
}
