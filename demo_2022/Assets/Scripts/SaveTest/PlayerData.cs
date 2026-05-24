using System;
using UnityEngine;
using System;


public class PlayerData : MonoBehaviour
{
    public Transform mainCamera;

    #region Fields

    //[SerializeField] string playerName = "Player Name";
    //[SerializeField] int level = 0;
    //[SerializeField] int coin = 0;
    [SerializeField] public float rotX,rotY,rotZ;
    [SerializeField] Vector3 position;


    //可序列化特性
    [System.Serializable]
    class SaveData
    {
        //public Quaternion direction;  json 不能序列化 Quaternion
        //拆分 保存四元数角度会出问题 ，用欧拉角更好

        public float rotX,rotY, rotZ;
        public Vector3 position;
        public Vector2 cameraRotation;
        public Quaternion GetRotation()
        {
            return Quaternion.Euler(rotX,rotY,rotZ);
        }
        
    }
   

    const string PLAYER_DATA_KEY = "PlayerData";
    const string PLAYER_DATA_FILE_NAME = "PlayerData.json";
    // 方法2：JSON保存（精度足够，更方便调试）
    

    #endregion

    #region Properties

    #endregion

    #region Save and Load

    public void Save()
    {
        //SaveByPlayerPrefs();
        SaveByJson();
    }

    

    public void Load()
    {
        //LoadFromByPlayerPrefs();
        LoadFromJson();
    }
    private void SaveByPlayerPrefs()
    {

        SaveSystem.SaveByPlayerPrefs(PLAYER_DATA_KEY, SavingData());// key value
    }
    private void LoadFromByPlayerPrefs()
    {
        var json = SaveSystem.LoadFromPlayerPrefs(PLAYER_DATA_KEY);
        var saveData = JsonUtility.FromJson<SaveData>(json);
        LoadData(saveData);
    }


    #endregion

    #region JSON

    void SaveByJson()
    {
        SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, SavingData());
        Debug.Log("Rotation" + transform.rotation);

        //SaveSystem.SaveByJson($"{System.DateTime.Now:yyyy.dd.M HH-mm-ss}.sav",SavingData());//按时名保存

    }
    void LoadFromJson()
    {
        var SaveData = SaveSystem.LoadFromJson<SaveData>(PLAYER_DATA_FILE_NAME);
        LoadData(SaveData);
    }


    #endregion

    #region Help Functions
    SaveData SavingData()
    {
        SaveData data = new SaveData();
        
        data.position = transform.position;
        Vector3 euler = transform.eulerAngles;
        data.rotX = euler.x;
        data.rotY = euler.y;
        data.rotZ = euler.z;
        data.position = transform.position;
        CameraRotation cam = mainCamera.GetComponent<CameraRotation>();
        if(cam != null)
        {
            data.cameraRotation = cam.GetRotationState();//获取旋转信息
        }
        
        return data;
    }

    void LoadData(SaveData saveData)
    {
        CameraRotation cam = mainCamera.GetComponent<CameraRotation>();

        //这里下一步会被cameraRotation的旋转覆盖
        transform.rotation = saveData.GetRotation();
        if (cam != null)
        {
            cam.SetRotationState(saveData.cameraRotation);//必须更新旋转信息;
        }
        transform.position = saveData.position;
        
    }

    #endregion
    [UnityEditor.MenuItem("Developer/Delete Player Data Prefs")]//可以在编辑器里点击，但是函数必须为静态的
    public static void DeletePlayerDataPrefs()
    {
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey(PLAYER_DATA_KEY);
    }
    [UnityEditor.MenuItem("Developer/Delete Player Data Save File")]
    public static void DeletePlayerDataSaveFile()
    {
        SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME);
    }



    private void Start()
    {
        Load();//加载上一次的位置
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Save();
            Debug.Log("Save Success");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Load();
            Debug.Log("Load Success");
        }
    }
}
