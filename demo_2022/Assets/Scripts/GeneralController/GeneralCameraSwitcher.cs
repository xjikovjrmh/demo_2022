using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCameraSwitcher : MonoBehaviour
{
    [Header("存放需要切换的车辆目标对象")]
    [Tooltip("可以存放各辆车的车身，或者只负责各辆车摄像机/控制器的父节点")]
    public List<GameObject> list = new List<GameObject>();

    [Header("控制模式与自由视角")]
    public ModeController modeController;
    public GameObject freeCameraObj;

    private int currentCarIndex = 0;
    private bool isFreeCameraActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // 游戏启动时，初始化只给列表第一个启用的观察视角，其余关闭
        UpdateActiveCar();
    }

    // Update is called once per frame
    void Update()
    {
        // 使用配置好的 modeController 判断 Tab 是否处于按下状态，如果没有配置则回退到原生按键检测
        bool isTabPressed = (modeController != null) 
            ? (modeController.mode == ModeController.Mode.Control) 
            : Input.GetKey(KeyCode.Tab);

        // 检测 Tab + X 切换（开启 / 关闭）自由视角
        if(isTabPressed && Input.GetKeyDown(KeyCode.X))
        {
            isFreeCameraActive = !isFreeCameraActive;
            UpdateActiveCar();
        }

        // 检测 Tab + G 切换观察的小车
        if(isTabPressed && Input.GetKeyDown(KeyCode.G))
        {
            SwitchToNextCar();
        }
    }

    private void SwitchToNextCar()
    {
        if (list == null || list.Count == 0) return;

        // 索引加1，取模实现循环（0, 1, 2 -> 0, 1, 2）
        currentCarIndex = (currentCarIndex + 1) % list.Count;

        UpdateActiveCar();
    }

    private void UpdateActiveCar()
    {
        // 先处理自由视角的显隐（只有当你拖入了自由视角对象时才生效）
        if (freeCameraObj != null)
        {
            freeCameraObj.SetActive(isFreeCameraActive);
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                // 如果当前正在自由视角下，所有关联车辆的摄像头和控制统统不应该处于被观察状态
                bool isActiveCar = (i == currentCarIndex) && !isFreeCameraActive;

                // 方法一：如果你传入的 list 是类似 "CameraPivot" 这种只负责相机和控制的父物体
                // 那么直接控制它们的显隐即可：
                // list[i].SetActive(isActiveCar);

                // 方法二（贴合你之前的逻辑）：如果 list 存放的是具体的整辆车（车不能隐藏，隐藏的应只是摄像功能）
                // 那么只搜索并开启/关闭车身下的 Camera 和 AudioListener 组件：
                Camera[] cams = list[i].GetComponentsInChildren<Camera>(true);
                foreach (Camera cam in cams)
                {
                    // 仅当是属于当前被观察车辆的相机时开启（注意这里依然可能会受到C键第一/第三视角控制的影响）
                    if (cam.gameObject.name == "FrontCamera" || cam.gameObject.name == "ThirdPartyCamera")
                    {
                        // 不要关闭整个 GameObject，否则会搞乱 CarCameraController 里的 activeSelf 判断
                        // 只需要关闭 Camera 和 AudioListener 组件即可
                        cam.enabled = isActiveCar;

                        AudioListener listener = cam.GetComponent<AudioListener>();
                        if (listener != null)
                        {
                            listener.enabled = isActiveCar;
                        }
                    }
                }

                // 另外，对于未被观察的车辆，还要禁止它们捕捉鼠标事件或切换摄像头的输入：
                MonoBehaviour[] allScripts = list[i].GetComponentsInChildren<MonoBehaviour>(true);
                foreach (MonoBehaviour script in allScripts)
                {
                    // 把你的相机控制、鼠标采集脚本在非观察状态下禁用，防冲突！
                    string scriptName = script.GetType().Name;
                    if (scriptName == "CameraController" ||
                        scriptName == "CarCameraController" ||
                        scriptName == "ParsingMouse")
                    {
                        script.enabled = isActiveCar;
                    }
                }
            }
        }
    }
}
