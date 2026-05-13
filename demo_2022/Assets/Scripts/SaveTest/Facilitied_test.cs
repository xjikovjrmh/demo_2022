using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facilitied_test : MonoBehaviour
{
    // Start is called before the first frame update
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            FacilityManager.Instance.AddFacility(new FacilityData
            {
                id = "pipe_001",
                prefabName = "Pipe_Straight",
                posX = 10,
                posY = 2,
                posZ = 5,
                rotX = 0,
                rotY = 90,
                rotZ = 0,
                scale = 1.0f,
                modelFileName = "pipe.obj"
            });
        }
    }

}
