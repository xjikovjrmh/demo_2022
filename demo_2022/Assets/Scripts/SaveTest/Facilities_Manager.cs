using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//引入插件
using GLTFast;
using System;
using System.Collections;
/// <summary>
/// 设施进度管理器 —— 挂到一个名为"ProgressManager"的空物体上
/// </summary>
/// 其他脚本可以通过FacilityManager.Instance
public class FacilityManager : MonoBehaviour
{
    public static FacilityManager Instance { get; private set; }
    private ProgressData progressData;//实际是是存储一堆facility的列表
    //声明事件 
    public event Action<string> OnFacilityDeleted;

    public event Action<string> OnFacilityRestored;
    public event Action<FacilityData> OnFacilityAdded;
    private bool isDirty = false;      // ← 新增  标记是否有修改
    private Coroutine autoSaveCoroutine; // 防抖协程 0.5秒后修改json数据

    public void SaveNow()
    {
        SaveProgress();
        isDirty = false;
    }
    private void TryAutoSave()
    {
        isDirty = true;
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
    }
    private IEnumerator AutoSaveRoutine() //添加using system.collections.generic;才能用
    {
        yield return new WaitForSeconds(0.5f);
        if(isDirty)
        {
            SaveProgress();
            isDirty = false;
        }
    }
    public bool DeleteFacility(string id)
    {
        FacilityData data =progressData.facilities.Find(f => f.id == id);
        if (data == null || data.isDeleted) return false;
        {
            //修改json数据
            data.isDeleted = true;
            isDirty = true;
            //保存
            TryAutoSave();
            //发送事件
            OnFacilityDeleted?.Invoke(id);
            return true;
        }
    }
    public bool RestoreFacility(string id)
    {
        FacilityData data = progressData.facilities.Find(f => f.id == id);
        if (data == null || !data.isDeleted) return false;// 不存在或者没有被删除的
        
        //恢复
        data.isDeleted = false;
        isDirty = true;
        //保存
        TryAutoSave();
        OnFacilityRestored?.Invoke(id);
        return true;

    }
    // 获取最近删除的 id ，从最近的开始，一个一个返回
    public string GetRecentDeletedFacilityId()
    {
        // 从 facilities 中找 isDeleted == true 且 addedTime 最新的
        FacilityData recent = null;
        foreach (var f in progressData.facilities)
        {
            if (!f.isDeleted) continue;  // 被删除的才继续
            if (recent == null || string.Compare(f.addedTime, recent.addedTime) > 0)
                recent = f;
        }
        return recent?.id;
    }



    private string Model_Path;
    private const string FILE_NAME = "progress.json";
    const string facility_path = "facility/facility";
    private GameObject facilityPrefab;

    private Vector3 currentPosition=Vector3.zero;


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
                    if (progressData.facilities[i].isDeleted) //被删除的不显示
                        continue;
                    Quaternion targetRotation = progressData.facilities[i].GetRotation();
                    
                    GameObject obj=Instantiate(facilityPrefab, progressData.facilities[i].GetPosition(), targetRotation);
                    //挂载脚本 身份标识
                    FacilityIdentity identity =obj.GetComponent<FacilityIdentity>();
                    if(identity == null)
                    {
                        identity = obj.AddComponent<FacilityIdentity>();
                    }
                    //设置id,从json读取
                    identity.Id = progressData.facilities[i].id;
                    
                    obj.name = progressData.facilities[i].id;
                    
                    //obj.tag = "facility";//添加标签
                }
                //加载资源      
            }
        }

    }

    private void OnApplicationQuit()
    {
        if (isDirty) SaveNow();  // 退出时跳过防抖，直接写盘
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
    public void RemoveFacility(string id)//因为传入的是id
    {
        // 查找是否存在
        FacilityData data = progressData.facilities.Find(f => f.id == id);
        if (data == null)
        {
            Debug.LogWarning($"设施 {id} 不存在，无法删除");
            return;
        }

        // 从列表中移除
        progressData.facilities.Remove(data);

        // 保存
        SaveProgress();

        Debug.Log($"已删除设施: {id}");
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