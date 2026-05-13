using System.Collections.Generic;
using System.IO;
using UnityEngine;
//引入插件
using GLTFast;
/// <summary>
/// 设施进度管理器 —— 挂到一个名为"ProgressManager"的空物体上
/// </summary>
public class FacilityManager : MonoBehaviour
{
    public static FacilityManager Instance { get; private set; }

    private ProgressData progressData;

    private string Model_Path;
    private const string FILE_NAME = "progress.json";
    const string facility_path = "facility/facility";
    private GameObject facilityPrefab;

    private Vector3 currentPosition=Vector3.zero;

    


    async void Start()
    {
        // 拼出你模型的实际路径
        string modelPath = Path.Combine(Application.persistentDataPath, "Models", "facility.glb");

        // 读取文件为二进制
        byte[] data = File.ReadAllBytes(modelPath);

        var loader = new GltfImport();
        // 2. 正确调用 Load 方法
        // 注意：不需要 <byte[]>，直接传 data 和 uri
        // data 是 byte[] 类型，uri 是文件路径
        bool success = await loader.Load(data, new System.Uri(modelPath));

        // 实例化
        if (success)
        {
            // 先实例化到当前物体的子级     InstantiateMainSceneAsync函数在指定的父物体下创建模型
            success = await loader.InstantiateMainSceneAsync(transform);

            if (success)
            {
                // 从 transform 的子物体中获取实例化的模型
                if (transform.childCount > 0)
                {
                    facilityPrefab = transform.GetChild(transform.childCount - 1).gameObject;
                    // 如果不需要作为子物体，可以先分离出来
                    facilityPrefab.transform.SetParent(null);
                    facilityPrefab.transform.position = Vector3.zero;//重置位置

                    //添加碰撞体
                    SetupParentMeshCollider(facilityPrefab);

                }
            }
        }
        else
        {
            Debug.LogError("加载失败");
            return;
        }


        //读取进度
        LoadProgress();
        if (facilityPrefab != null)
        {
            int count = progressData.facilities.Count;//获取已有的资源总数
            for (int i = 0; i < count; i++)
            {
                {
                    Quaternion targetRotation = progressData.facilities[i].GetRotation();
                    Instantiate(facilityPrefab, progressData.facilities[i].GetPosition(), targetRotation);
                }
                //加载资源      
            }

        }

    }

    private void SetupParentMeshCollider(GameObject parent)
    {
        // 1. 获取所有子物体的 MeshFilter
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();

        if (meshFilters.Length == 0)
        {
            Debug.LogWarning($"{parent.name} 没有找到任何 MeshFilter");
            return;
        }

        // 2. 合并所有子网格为一个网格（用于碰撞）
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh == null) continue;

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = parent.transform.worldToLocalMatrix *
                                  meshFilters[i].transform.localToWorldMatrix;
        }

        // 3. 创建合并后的网格
        Mesh combinedMesh = new Mesh();
        combinedMesh.name = "CombinedCollider";
        combinedMesh.CombineMeshes(combine);

        // 4. 添加 MeshCollider 并赋值
        MeshCollider meshCollider = parent.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;
        meshCollider.convex = true;  // 🔧 射线检测必须设置 convex = true

        // 5. 可选：禁用子物体的渲染器（如果有单独的碰撞需求可以保留）
        Debug.Log($"✅ 已为 {parent.name} 添加合并碰撞体，包含 {meshFilters.Length} 个子网格");
    }
    //private void SetupCollider(GameObject model)
    //{
    //    // 检查模型本身是否有 MeshFilter（无子物体情况）
    //    MeshFilter selfMeshFilter = model.GetComponent<MeshFilter>();

    //    if (selfMeshFilter != null && selfMeshFilter.sharedMesh != null)
    //    {
    //        // 情况1：模型本身有网格（无子物体）
    //        MeshCollider mc = model.AddComponent<MeshCollider>();
    //        mc.sharedMesh = selfMeshFilter.sharedMesh;
    //        mc.convex = true;
    //        Debug.Log($"✅ 单物体模型，直接添加碰撞体");
    //    }
    //    else
    //    {
    //        // 情况2：模型有子物体，尝试合并网格
    //        MeshFilter[] childMeshFilters = model.GetComponentsInChildren<MeshFilter>();

    //        if (childMeshFilters.Length > 1)
    //        {
    //            // 多个子物体，合并网格
    //            SetupParentMeshCollider(model);
    //        }
    //        else if (childMeshFilters.Length == 1)
    //        {
    //            // 只有一个子物体，在其上添加碰撞体（推荐）
    //            MeshCollider mc = childMeshFilters[0].gameObject.AddComponent<MeshCollider>();
    //            mc.sharedMesh = childMeshFilters[0].sharedMesh;
    //            mc.convex = true;
    //            Debug.Log($"✅ 单子物体模型，在子物体上添加碰撞体");
    //        }
    //        else
    //        {
    //            // 退路：使用 BoxCollider
    //            SetupParentBoxCollider(model);
    //        }
    //    }
    //}






    void Awake()
    {
        //获取路径
        Model_Path = Path.Combine(Application.persistentDataPath, "Models");
        // 单例
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ========== 添加设施（由外部调用） ==========
    public void AddFacility(FacilityData data)
    {
        // 去重检查 //f 是列表的id ，data.id 是要添加的id，外部调用时，列表还没有存储data
        if (progressData.facilities.Exists(f => f.id == data.id))
        {
            Debug.LogWarning($"设施 {data.id} 已存在，跳过");
            return;
        }

        if (string.IsNullOrEmpty(data.addedTime))
        {
            data.addedTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        progressData.facilities.Add(data);
        SaveProgress();
        Debug.Log($"已添加设施: {data.id}");
    }

    // ========== 获取所有设施（供恢复场景用） ==========
    public List<FacilityData> GetAllFacilities()
    {
        return progressData.facilities;
    }

    // ========== 内部：读取 ==========
    void LoadProgress()
    {
        if (SaveSystem.SaveFileExists(FILE_NAME))
        {
            progressData = SaveSystem.LoadFromJson<ProgressData>(FILE_NAME);
            Debug.Log($"读取进度：{progressData.facilities.Count} 个设施");
        }
        else
        {
            progressData = new ProgressData();
            Debug.Log("没有进度文件，初始化为空");
        }
    }

    // ========== 内部：保存 ==========
    void SaveProgress()
    {
        SaveSystem.SaveByJson(FILE_NAME, progressData);
    }
}