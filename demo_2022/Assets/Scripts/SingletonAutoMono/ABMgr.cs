using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABMgr : SingletonAutoMono<ABMgr>
{
    // Start is called before the first frame update
    //字典来存加载过的ab包
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    private AssetBundle mainAB = null; //记录主包是否加载
    private AssetBundleManifest manifest = null;//依赖包信息

    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    private string MainABName
    {
        get
        {
#if UNITY_IOS
return "IOS";
#elif UNITY_ANDROID
return "Android";
        }
#else
            return "PC";
#endif
        }
    }

    public void LoadAB(string abName)
    {
        //加载ab包
        //加载主包
        if (mainAB == null)//主包只能加载一次
        {
            //加载主包
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            //加载依赖包
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        AssetBundle ab = null; //临时存包

        string[] strs = manifest.GetAllDependencies(abName);

        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))//不包含说明没有存
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }

        //加载资源包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }


    }


    public Object LoadRes(string abName, string resName)
    {
        LoadAB(abName);
        //
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
        {
            return obj;
        }


    }

    public Object LoadRes(string abName, string resName, System.Type type)
    {
        LoadAB(abName);
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
        {
            return obj;
        }
    }

    //这里是T 不是Object
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
        {
            return obj;
        }
    }
    //单个包卸载
    public void Unload(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //所有包卸载
    public void Clear()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
