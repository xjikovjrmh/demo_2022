using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGenerateTunnel : MonoBehaviour
{
    public Transform player; // 玩家或摄像机的Transform
    public float tunnelLength = 50f;
    //public float recycleDistance = 100f; // 超出多少距离回收
    private List<GameObject> activeTunnels = new List<GameObject>();


    private GameObject tunnelPrefab;
    private Transform lastTunnel;
    private Coroutine generateCoroutine;
    private bool isGenerating = false;

    private void Start()
    {
        //起点第一个隧道初始化
        tunnelPrefab =ABMgr.GetInstance().LoadRes<GameObject>("model", "tunnel");
        tunnelPrefab.SetActive(false);//隐藏起来

        //第二个
        //tunnelPrefab = ABMgr.GetInstance().LoadRes<GameObject>("model", "tunnel");
        //tunnelPrefab.transform.position += new Vector3(0, 0, 50);
        tunnelPrefab.name = "model/tunnel";

        lastTunnel = tunnelPrefab.transform;//要赋值
        


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isGenerating)
            {
                generateCoroutine = StartCoroutine(AutoGenerate());
                isGenerating = true;
            }
            else
            {
                StopCoroutine(generateCoroutine);
                isGenerating = false;
            }
        }
    }
    private IEnumerator AutoGenerate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Vector3 nextPos = lastTunnel.position + new Vector3(0, 0, tunnelLength);

            GameObject obj = Instantiate(tunnelPrefab);
            obj.SetActive(true);//显示

            obj.name = tunnelPrefab.name;
            obj.transform.position = nextPos;
            lastTunnel = obj.transform;
            activeTunnels.Add(obj);
            
        }
    }
}
