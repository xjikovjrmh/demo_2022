using System.Collections.Generic;
using UnityEngine;

public class HighlightSystem2 :MonoBehaviour//要继承MonoBehaviour否则不能用泛型
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public static HighlightSystem2 Instance { get; private set;}

    public Color defaultColor = Color.red;
    public float defaultWidth = 5f;

    //缓存已添加Outline的物体 ，避免GetComponent重复调用
    private Dictionary<GameObject,Outline>outlineCache = new Dictionary<GameObject,Outline>();
    // 自动初始化
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInit()
    {
        if(Instance==null)
        {
            GameObject obj = new GameObject("HighlightSystem2");
            Instance = obj.AddComponent<HighlightSystem2>();
            Object.DontDestroyOnLoad(obj);
        }
    }

    //启用高亮
    public void EnableHightlight(GameObject obj, Color? color= null,float?width=null)
    {
        if(obj==null) return;

        //获取或添加Outline组件  //如果没有在字典中缓存，则获取组件并添加到缓存中
        if(!outlineCache.TryGetValue(obj,out Outline outline))
        {
            outline = obj.GetComponent<Outline>();
            if(outline==null)//没有则添加，避免重复添加
            {
                outline = obj.AddComponent<Outline>();
            }
            outlineCache[obj]=outline;//添加到缓存
        }

// 设置参数
        outline.OutlineColor = color ?? defaultColor;
        outline.OutlineWidth = width ?? defaultWidth;
        outline.enabled = true;

    }
//禁用高亮(保存组件)
    public void DisableHighlight(GameObject obj)
    {
        if(obj==null) return;

        //从缓存中获取Outline组件    
        if(outlineCache.TryGetValue(obj,out Outline outline))
        {
            outline.enabled = false;
        }
    }
    // 完全移除高亮（销毁组件 + 清理缓存）
    public void RemoveHighlight(GameObject obj)
    {
        if (obj == null || !outlineCache.TryGetValue(obj, out Outline ol)) return;
        
        Destroy(ol);
        outlineCache.Remove(obj);
    }
    // 禁用所有高亮（例如打开 UI 时）
    public void DisableAllHighlights()
    {
        foreach (var kvp in outlineCache)
        {
            if (kvp.Value != null)
                kvp.Value.enabled = false;
        }
    }
    // 可选：清理无效引用（防止内存泄漏）
    public void Cleanup()
    {
        var keysToRemove = new List<GameObject>();
        foreach (var kvp in outlineCache)
        {
            if (kvp.Key == null || kvp.Value == null)
                keysToRemove.Add(kvp.Key);
        }
        foreach (var key in keysToRemove)
            outlineCache.Remove(key);
    }
    

}

