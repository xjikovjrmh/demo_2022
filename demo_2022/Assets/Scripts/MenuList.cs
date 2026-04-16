using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuList : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuList;//将菜单列表视为GameObject 方便激活失活
    //menuList存放Esc 的画布
    private bool menuKey = true;
    private Mouse_Touch mouse_Touch;
    private void Start()
    {
        mouse_Touch= GetComponent<Mouse_Touch>();
    }

    private void Update()
    {
        if (menuKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuKey = false;
                Time.timeScale = (0);//时间暂停
                //后序有声音也要处理
                //取消鼠标锁定
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //mouseTouch也要更改
                mouse_Touch.enabled = false;

                //eventSystem响应 解决因为cursor锁定导致点击返回后重新打开菜单highlight不显示的问题
                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }

            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            menuList.SetActive(false);
            menuKey = true;
            Time.timeScale = (1);//时间恢复正常
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouse_Touch.enabled = true;
        }


        
    }
    public void Return()//返回场景
    {
        menuList.SetActive(false);
        menuKey = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouse_Touch.enabled = true;

    }
    public void Restart()//重新开始
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 2. 如果是打包后的程序（Windows/Mac/Linux/Android/iOS）
            Application.Quit();
#endif
    }

}
