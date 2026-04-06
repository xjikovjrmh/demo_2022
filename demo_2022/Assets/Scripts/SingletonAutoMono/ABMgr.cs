using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



//同步加载


public class ABMgr : SingletonAutoMono<ABMgr>
{
    // Start is called before the first frame update
    //字典来存加载过的ab包
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    private AssetBundle mainAB = null; //记录主包是否加载
    private AssetBundleManifest manifest = null;//依赖包信息

    //路径可能经常会变，用属性
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
    //封装函数，作用包括加载主包，依赖包，记录加载状态
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

        //最终加载资源包abName
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }


    }


    public Object LoadRes(string abName, string resName)
    {
        LoadAB(abName);
        //获得资源包的资源,并实例化
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
    //通常建议用泛型加载资源包
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
    //异步加载
    //ab包没有使用异步加载，从ab包中加载资源时使用异步
                                                        //委托
    public void LoadResAsync(string abName,string resName,UnityAction<Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName,resName,callback));
    }
    private IEnumerator ReallyLoadResAsync(string abName,string resName, UnityAction<Object> callback)
    {
        LoadAB(abName);
        //获得资源包的资源,并实例化
        //这里返回值要变                       API要变
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        //判断是否是GameObject
        //如果是，直接实例化，返回给外部
        //异步加载结束后，通过委托传递给外部使用
        if (abr.asset is GameObject)
        {
            callback (Instantiate(abr.asset));  //对于GameObject对像，要实例化
        }
        else  //
        {
            callback(abr.asset);//对于非GameObject对像，比如textures不用实例化，直接赋值
        }
        yield return null;
    }
    //根据类型type加载
    public void LoadResAsync(string abName, string resName,System.Type type, UnityAction<Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName,type, callback));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName,System.Type type, UnityAction<Object> callback)
    {
        LoadAB(abName);
        //获得资源包的资源,并实例化
        //这里返回值要变                       API要变
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName,type);
        yield return abr;
        //判断是否是GameObject
        //如果是，直接实例化，返回给外部
        //异步加载结束后，通过委托传递给外部使用
        if (abr.asset is GameObject)
        {
            callback(Instantiate(abr.asset));  //对于GameObject对像，要实例化
        }
        else  //
        {
            callback(abr.asset);//对于非GameObject对像，比如textures不用实例化，直接赋值
        }
        yield return null;
    }
    //根据泛型加载
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback)  where T : Object//加约束
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callback));
    }
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object
    {
        LoadAB(abName);
        //获得资源包的资源,并实例化
        //这里返回值要变                       API要变
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        //判断是否是GameObject
        //如果是，直接实例化，返回给外部
        //异步加载结束后，通过委托传递给外部使用
        if (abr.asset is GameObject)
        {
            callback(Instantiate((abr.asset) as T));  //对于GameObject对像，要实例化
        }
        else  //
        {
            callback(abr.asset as T);//对于非GameObject对像，比如textures不用实例化，直接赋值
        }
        yield return null;
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
