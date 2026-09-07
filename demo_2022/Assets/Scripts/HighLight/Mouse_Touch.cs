using System.Collections.Generic;
using UnityEngine;

public class Mouse_Touch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 已选中物体列表
    private List<GameObject> selectedobjs = new List<GameObject>();
    public Camera cam;
    public LayerMask raycastLayer; // 
    //这里默认层级是default ，也就是新加入场景的物体都可以挂载脚本（除了其他层级，比如tram，防止被删除）
    //private Stack<GameObject> resetobjs=new Stack<GameObject>();
    //id 到GameObject的映射
    private Dictionary<string, GameObject> hiddenObjects = new Dictionary<string, GameObject>();
    //使用HighLightSystem2可以手动添加高亮脚本，不用在打包时添加
    private void Start()//订阅移入Start
    {
        //订阅事件
        FacilityManager.Instance.OnFacilityDeleted += OnFacilityDeletedHandler;
        FacilityManager.Instance.OnFacilityRestored += OnFacilityRestoredHandler;
    }
    private void OnDestroy()//对应start订阅
    {
        //取消订阅 
        if (FacilityManager.Instance != null)
        {
            FacilityManager.Instance.OnFacilityDeleted -= OnFacilityDeletedHandler;
            FacilityManager.Instance.OnFacilityRestored -= OnFacilityRestoredHandler;
        }

    }
    private void OnFacilityDeletedHandler(string id)
    {
        //从选中列表中找到物体id，隐藏   facilityIdentity让物体可以删除
        GameObject obj = selectedobjs.Find(o =>
        o.GetComponent<FacilityIdentity>()?.Id == id);
        if (obj != null)
        {
            hiddenObjects[id] = obj;
            obj.SetActive(false);
            //移除高亮
            HighlightSystem2.Instance.RemoveHighlight(obj);
            selectedobjs.Remove(obj);
        }

    }
    private void OnFacilityRestoredHandler(string id)
    {
        //从隐藏列表中 找到物体id，恢复
        if (hiddenObjects.TryGetValue(id, out GameObject obj))
        {
            obj.SetActive(true);
            //移除字典
            hiddenObjects.Remove(id);
            selectedobjs.Add(obj);// 重新加入选中列表
            HighlightSystem2.Instance.EnableHightlight(obj, Color.Lerp(Color.red, Color.yellow, 0.5f), 10f);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //鼠标左键按下
        {

            //out 额外返回信息
            if (TryRayCast(out RaycastHit hit))//检测到物体   如果没有击中物体将不能直接访问.gameObject，会报错
            {
                GameObject clickedObj = hit.collider.gameObject;
                if (selectedobjs.Contains(clickedObj))//已选中则取消选中
                {
                    HighlightSystem2.Instance.DisableHighlight(clickedObj);
                    selectedobjs.Remove(clickedObj);//移除
                    Debug.Log("取消选中物体:" + clickedObj.name);
                }
                else
                {                                                     //橙色
                    HighlightSystem2.Instance.EnableHightlight(clickedObj, Color.Lerp(Color.red, Color.yellow, 0.5f), 10f);
                    selectedobjs.Add(clickedObj);//添加
                    Debug.Log("选中物体:" + clickedObj.name);
                }


            }
        }

        // 🗑️ Delete 键：删除或隐藏选中物体
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (selectedobjs.Count > 0)
            {
                DeleteOrHideSelectedObject();
            }
            else
            {
                Debug.Log("未选中任何物体，无法删除或隐藏。");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            string id = FacilityManager.Instance.GetRecentDeletedFacilityId();
            if (id != null)
            {
                FacilityManager.Instance.RestoreFacility(id);
            }
            else
            {
                Debug.Log("没有最近删除的物体，无法恢复。");
            }
        }

    }

    private void DeleteOrHideSelectedObject()
    {
        if (selectedobjs.Count == 0) return;
        //遍历选中的物体取id调用 manager
        for (int i = selectedobjs.Count - 1; i >= 0; i--)
        {
            GameObject obj = selectedobjs[i];
            if (obj == null) continue;
            //新逻辑 获取id 
            FacilityIdentity identity = obj.GetComponent<FacilityIdentity>();
            if (identity != null)
            {
                //通过id 删除（在这里修改json的数据，没有修改状态）
                //这里通过manager 广播通知  OnFacilityDeleted?.Invoke(id);
                //软删除
                FacilityManager.Instance.DeleteFacility(identity.Id);
                //如果需要硬删除，可以直接销毁物体
                // FacilityManager.Instance.RemoveFacility(identity.Id);
            }
            string objName = obj.name;
            //修改状态
            //obj.SetActive(false); // 隐藏物体

            //resetobjs.Push(obj);//加入待复原列表
            Debug.Log("已隐藏物体: " + objName);

            //HighlightSystem2.Instance.RemoveHighlight(obj); // 移除高亮
        }
        //selectedobjs.Clear(); // 清空选择
    }
    //摄像检测封装，
    private bool TryRayCast(out RaycastHit hit)
    {
        //将屏幕坐标转为世界空间射线
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); //从摄像机发出一条经过鼠标位置的射线
        //调试代码
        //Debug.DrawRay(ray.origin, ray.direction * 2000f, Color.red, 2f);

        return Physics.Raycast(ray, out hit, 2000f, raycastLayer);  //如果命中，则返回 true，并将命中的信息存储在 hit 中
                                                                    //最大距离限制     //检测层

    }



}
