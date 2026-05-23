using System;
using UnityEngine;
using System;


public class PlayerData : MonoBehaviour
{
    #region Fields

    //[SerializeField] string playerName = "Player Name";
    //[SerializeField] int level = 0;
    //[SerializeField] int coin = 0;
    [SerializeField] Quaternion direction;
    [SerializeField] Vector3 position;


    //可序列化特性
    [System.Serializable]
    class SaveData
    {
        public Quaternion direction;
        public Vector3 position;
    }
    const string PLAYER_DATA_KEY = "PlayerData";
    const string PLAYER_DATA_FILE_NAME = "PlayerData.sav";

    #endregion

    #region Properties
    public Quaternion Direction =>direction;
    public Vector3 Position => transform.position;

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

        return new SaveData
        {

            direction = transform.rotation,   //try
            position = transform.position
        };
    }

    void LoadData(SaveData saveData)
    {
        transform.rotation = saveData.direction;
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
