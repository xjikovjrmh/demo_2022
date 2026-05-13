using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单条设施数据
/// </summary>
[Serializable]
public class FacilityData
{
    public string id;
    public string prefabName;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public float scale;
    public string modelFileName;
    public string addedTime;

    // 方便操作的辅助方法（可选）
    public Vector3 GetPosition()
    {
        return new Vector3(posX, posY, posZ);
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(rotX, rotY, rotZ);
    }
}

/// <summary>
/// 整体进度容器
/// </summary>
[Serializable]
public class ProgressData
{
    public List<FacilityData> facilities = new List<FacilityData>();
}