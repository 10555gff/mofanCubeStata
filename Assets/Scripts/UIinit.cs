using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIinit : MonoBehaviour
{
    public static UIinit Instance = null;
    public Transform Canvas_ui;
    Button[] btns;
    Button btnUp;
    Button btnDown;

    Text txt_restore;
    Text txt_tip;
    Text txt_target;
    Image mo_image;

    string[] btn_name = { "F", "B", "R", "L", "U", "D",
                          "f", "b", "r", "l", "u", "d",
                          "y", "z", "x", "M", "S", "E"};
    //暂缓按钮的文本控件
    Text btn_contxt;
    //暂缓按钮的文本内容
    string t1;
    //按钮的类型
    public static int btn_type=1;
    void Start()
    {
        Instance = this;
        //批量绑定Button
        btns = new Button[btn_name.Length];
        for (int i = 0; i < btn_name.Length; i++)
        {
            btns[i] = Canvas_ui.GetChild(i).GetComponent<Button>();
            btns[i].onClick.AddListener(btnOnClick);
        }
        //面板绑定
        foreach (Transform t in Canvas_ui.transform.Find("Panel").GetComponentsInChildren<Transform>())
        {
            switch (t.name)
            {
                case "txt_restore":
                    txt_restore = t.GetComponent<Text>();
                    break;
                case "txt_tip":
                    txt_tip = t.GetComponent<Text>();
                    break;
                case "txt_target":
                    txt_target = t.GetComponent<Text>();
                    break;
                case "mo_image":
                    mo_image = t.GetComponent<Image>();
                    break;
                case "btnUp":
                    btnUp = t.GetComponent<Button>();
                    btnUp.onClick.AddListener(delegate ()
                    {
                        MofanMixCube.Instance.switchFormula(1);
                    });
                    break;
                case "btnDown":
                    btnDown = t.GetComponent<Button>();
                    btnDown.onClick.AddListener(delegate ()
                    {
                        MofanMixCube.Instance.switchFormula(-1);
                    });
                    break;
            }
        }

    }
    //Button事件组处理
    private void btnOnClick()
    {
        //转完才能再转
        if (GameManager.Instance.isFinish)
        {
            t1 = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name;
            switch (t1.Substring(t1.Length - 1, 1))
            {
                case "x":
                    GameManager.Instance.mofanBtnOnclick(t1.Substring(0, 1), GameManager.Axis.X,false);
                    break;
                case "y":
                    GameManager.Instance.mofanBtnOnclick(t1.Substring(0, 1), GameManager.Axis.Y,false);
                    break;
                case "z":
                    GameManager.Instance.mofanBtnOnclick(t1.Substring(0, 1), GameManager.Axis.Z,false);
                    break;
                case "o":
                    GameManager.Instance.mofanBtnOnclick(t1.Substring(0, 1), GameManager.Axis.O,false);
                    break;
            }
        }
    }

    void Update()
    {
        //魔方旋转类型转换
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            changeBtnText(2);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            changeBtnText(1);
        }
    }
    void changeBtnText(int type)
    {
        //要传参的按钮类型
        btn_type = type;
        //传参给类MofanMarkCreate改变标示符
        GameObject.Find("MarkCanvas").SendMessage("ChangMarkValue");
        //改变按钮文本
        for (int i = 0; i < btn_name.Length; i++)
        {
            //获取按钮的文本控件
            btn_contxt = btns[i].GetComponentInChildren<Text>();
            switch (type)
            {
                case 1:
                    btn_contxt.text = btn_name[i];
                    break;
                case 2:
                    btn_contxt.text = btn_name[i]+"'";
                    break;
            }
            
        }
    }


    public void ChangeValue(string s1,string s2,string s3, Sprite sp)
    {
        txt_restore.text = s1;
        txt_target.text = s2;
        txt_tip.text = s3;
        mo_image.sprite =sp;
    }
    public void ChangeValue(string s1,string s2)
    {
        txt_restore.text = s1;
        txt_target.text = s2;
    }
    public void ChangeValue(string s3, Sprite sp)
    {
        txt_tip.text = s3;
        mo_image.sprite = sp;
    }
}
