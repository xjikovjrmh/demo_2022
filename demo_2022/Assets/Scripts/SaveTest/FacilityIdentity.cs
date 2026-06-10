// 挂载到设施预制体上
using UnityEngine;

public class FacilityIdentity : MonoBehaviour
{
    [SerializeField] private string id;

    public string Id
    {
        get => id;
        set
        {
            id = value;
            gameObject.name = value;  // 直接用 string ID 作为物体名

        }
    }
}

//RaycastHit hit;
//if (Physics.Raycast(ray, out hit))
//{
//    FacilityIdentity fac = hit.collider.GetComponent<FacilityIdentity>();
//    if (fac != null)
//    {
//        Debug.Log($"命中设施 ID: {fac.Id}");  // 输出 "pipe_001"

//        // 直接用 string ID 查你的数据
//        YourData data = dataManager.GetData(fac.Id);
//    }
//}
