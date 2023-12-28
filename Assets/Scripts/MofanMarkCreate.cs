using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MofanMarkCreate : MonoBehaviour
{
    List<GameObject> MarkPrefabList = new List<GameObject>();

    string[] mark_name = { "F", "B", "R", "L", "U", "D", "↻", "↺" };
    Vector3[] mark_position = { new Vector3(0, 0, -1.53f), new Vector3(0, 0, 1.53f),
                                new Vector3(1.53f, 0, 0),new Vector3(-1.53f, 0, 0),
                                new Vector3(0, 1.53f, 0),new Vector3(0, -1.53f, 0)};

    Vector3[] mark_rotation= { new Vector3(0, 0, 0), new Vector3(0, -180, 0),
                                new Vector3(0, -90, 0),new Vector3(0, 90, 0),
                                new Vector3(90, 0, 0),new Vector3(-90, 0, 0)};
    //标示创建暂缓区
    Text t1,t2,text;
    GameObject obj;
    //标示文本控件暂缓区
    Text[] text_arr=new Text[6];
    //记录上一次点击时间
    float lastTime = 0;

    void Start()
    {
        MarkCreate();
        //标示文本控件绑定初始化
        for (int i = 0; i < text_arr.Length; i++)
        {
            text_arr[i]= transform.Find(mark_name[i]).GetComponent<Text>();
            EventTriggerListener.Get(text_arr[i].gameObject).onClick = DoubleClick;
        }
    }
    void DoubleClick(GameObject go)
    {
        //当前时间和变量记录的上次点击时间进行比较,如果间隔短，就认为是个双击
        if (Time.realtimeSinceStartup - lastTime < 0.3f && MofanMixCube.setClick)
        {
            //在这里监听按钮的双击事件
            switch (go.name)
            {
                case "F":
                    MofanMixCube.Instance.CheckFan();
                    break;
                case "B":
                    GameManager.Instance.stopCor = StartCoroutine(GameManager.Instance.InMStrMfan("y y", true));
                    break;
                case "R":
                    GameManager.Instance.InStrMofan("y", true);
                    break;
                case "L":
                    GameManager.Instance.InStrMofan("y'", true);
                    break;
                case "U":
                    GameManager.Instance.InStrMofan("x'", true);
                    break;
                case "D":
                    GameManager.Instance.InStrMofan("x", true);
                    break;
            }
            GameObject.Find("Main Camera").SendMessage("InFAngles");
        }
        //记录下当前的时间
        lastTime = Time.realtimeSinceStartup;
    }

    void MarkCreate()
    {
        //魔方标示的创建
        for (int i = 0; i < mark_position.Length; i++)
        {
            MarkPrefabList.Add(new GameObject(mark_name[i]));
            //设置物体的Scale
            MarkPrefabList[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            //设置物体的位置
            MarkPrefabList[i].transform.Translate(mark_position[i]);
            MarkPrefabList[i].transform.localEulerAngles = mark_rotation[i];
            //给新的游戏对象添加Text组件
            text = MarkPrefabList[i].AddComponent<Text>();
            //使用默认的字体
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            //文本内容
            text.text = mark_name[i];
            //文本排列方式
            text.alignment = TextAnchor.MiddleCenter;
            //文本字体颜色
            text.color = Color.black;
            //文本字体大小
            text.fontSize = 32;
            //文本字体样式
            text.fontStyle = FontStyle.Bold;
            //成为子对象
            MarkPrefabList[i].transform.SetParent(this.transform);
            //复制自己然后作为顺逆时针的图标
            obj = Instantiate(MarkPrefabList[i], MarkPrefabList[i].transform.position, MarkPrefabList[i].transform.rotation, MarkPrefabList[i].transform);
            obj.transform.localEulerAngles = new Vector3(0, 0, 180);
            text = obj.GetComponentInChildren<Text>();
            text.text = mark_name[6];
            text.fontSize = 90;
        }
    }
    
    public void ChangMarkValue()
    {
        //改变标示的文本内容
        for (int i = 0; i < mark_position.Length; i++)
        {
            //获取文本控件
            t1 = MarkPrefabList[i].GetComponentInChildren<Text>();
            //获取顺逆时针文本控件
            t2 = MarkPrefabList[i].GetComponentsInChildren<Text>()[1];
            switch (UIinit.btn_type)
            {
                case 1:
                    t1.text = mark_name[i];
                    t2.text = mark_name[6];
                    break;
                case 2:
                    t1.text = mark_name[i] + "'";
                    t2.text = mark_name[7];
                    break;
            }

        }
    }
}
