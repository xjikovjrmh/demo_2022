using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    // Start is called before the first frame update
    #region PlayerPrefs
    public static void SaveByPlayerPrefs(string key,object data)
    {
        var json = JsonUtility.ToJson(data);//可以存储任何对象 第二个参数 true 格式化。如果不是为了阅读建议改为false
        Debug.Log(json);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

#if UNITY_EDITOR
    Debug.Log("已保存数据: " + key);
#endif
    }
    public static string LoadFromPlayerPrefs(string key)
    {
        //防止空数据报错
        if (PlayerPrefs.HasKey(key))
        {
            var json = PlayerPrefs.GetString(key);
            return json;
        }
        else
        {
            return null;
        }
    }
    #endregion





    //基于Json的存档系统


    #region JSON
    public static bool SaveFileExists(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        return File.Exists(path);
    }


    public static void SaveByJson(string saveFileName,object data)
    {
        var json = JsonUtility.ToJson(data,true);  //true 是为了可读性
        //这样可以适用于Windows和Android
        var path =Path.Combine(Application.persistentDataPath,saveFileName);

#if UNITY_EDITOR
        Debug.Log($"Successfully saved data to {path}");
#endif
        try
        {
            File.WriteAllText(path, json);//会覆写
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR 
            Debug.LogError($"Failed to save data to {path}.\n{exception}");
#endif
        }
    }

    public static T LoadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
#if UNITY_EDITOR
        Debug.Log($"Successfully load data from {path}");
#endif
        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to load data from {path}.\n{exception}");
#endif
            return default;
        }
    }

    #endregion

    #region Deleting
    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            File.Delete(path);
        }catch(System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to delete save file {path}.\n{exception}");
#endif
        }
    }


    #endregion

}
