using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyBoardInput : MonoBehaviour
{
    private Vector3 spawnOffset = new Vector3(0, 0, 5); // 每次生成的偏移
    private GameObject CylinderPrefab;
    
    [Header("生成起始点（建议拖入空物体作为参考点）")]
    private Vector3 spawnOriginPos=new Vector3(0,0,0); // 可选：用场景中的一个空物体作为起点

    private Vector3 nextSpawnPosition; // ✅ 用 Vector3 记录下一个生成位置
    public bool isAutoSpawning = false;
    private Coroutine spawnCoroutine;
    public float deltaTime = 1f;

    private int interactiveLayer;
    void Start()
    {
        // 初始化生成起点：优先用 spawnOrigin，否则用当前脚本所在位置
        nextSpawnPosition = spawnOriginPos+spawnOffset;
        interactiveLayer = LayerMask.NameToLayer("Interactive");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleAutoSpawn();
        }
    }

    void ToggleAutoSpawn()
    {
        if (isAutoSpawning)
        {
            StopCoroutine(spawnCoroutine);
            isAutoSpawning = false;
            Debug.Log("自动生成已停止");
        }
        else
        {
            if (CylinderPrefab == null)
            {
                //仅加载预制体资源，不做任何修改
                CylinderPrefab = ABMgr.GetInstance().LoadRes<GameObject>("model", "tunnel");
                //添加碰撞体
                CylinderPrefab.AddComponent<MeshCollider>();
                CylinderPrefab.transform.rotation=Quaternion.Euler(0,0,0);
                
                if (CylinderPrefab == null)
                {
                    Debug.LogError("无法加载 Cylinder 预制体！");
                    return;
                }
            }

            spawnCoroutine = StartCoroutine(AutoSpawnRoutine());
            isAutoSpawning = true;
            Debug.Log("自动生成已启动");
        }
    }

    IEnumerator AutoSpawnRoutine()
    {
        // 层ID异常时直接停止协程
        if (interactiveLayer == -1 || CylinderPrefab == null) yield break;
        
        while (true)
        {
            Quaternion cylinderRotation = Quaternion.Euler(0, 0f, 0f);
            // 2. 实例化后，仅修改场景中的实例对象
            GameObject spawnedObj = Instantiate(CylinderPrefab, nextSpawnPosition, cylinderRotation);
            
            // 给实例设置交互层（递归设置所有子物体，否则子物体层不对，射线检测不到）
            SetLayerRecursive(spawnedObj.transform, interactiveLayer);
            // 若预设体未提前加碰撞体，可在此给实例添加（建议提前加，性能更好）
            if (spawnedObj.GetComponent<MeshCollider>() == null)
            {
                MeshCollider collider = spawnedObj.AddComponent<MeshCollider>();
                // collider.convex = true; // 必须勾
            }

            // 累加下一次生成位置
            nextSpawnPosition += spawnOffset;

            yield return new WaitForSeconds(deltaTime);
        }
    }

    // 递归设置物体及所有子物体的层级（关键，否则子物体层不对）
    private void SetLayerRecursive(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        foreach (Transform child in trans)
        {
            SetLayerRecursive(child, layer);
        }
    }
    
}