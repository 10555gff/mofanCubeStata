using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 魔方结构体
/// </summary>
public struct CubeStruct
{
    public Vector3 mofanPos;//魔方位置
    public Vector3 mofanRot;//魔方旋转角度
}

public class MofanMixCube : MonoBehaviour
{
    public static MofanMixCube Instance = null;
    GameObject btnCanvas,miCamera;
    Button[] buttons;
    public static bool setClick = true;
    //按钮名字
    string[] btnName = { "mstateMix", "mstateSave", "mstateRecovery" , "MofanCheck", "MofanRestore" };
    //打乱魔方字符定义
    string[] mxi_strs = { "F","B","R","L","U","D","E","M","S",
                          "f","b","r","l","u","d"};
    //F2L颜色注释
    string[] f2lColour = { "红绿", "绿橙", "橙蓝" , "蓝红" };
    //还原块类型选择
    int reChoose = 0;


    //还原魔方公式定义
    //41个公式
    string[] F2L = { //1-5
                      "R U' R' U y' R' U U R U' U' R' U R",
                     "U R U' R' U' y' R' U R",
                     "R' F' R U R U' R' F",
                     "R U R' U' R U U R' U' R U R'",
                     "R F U R U' R' F' U' R'",
                     //6-10
                     "y' R' U' R U R' U' R",
                     "R U' R' U R U' R'",
                     "R U' R' U R U' U' R' U R U' R'",
                     "R U F R U R' U' F' R'",
                     "y' R' U R U' R' U R",
                     //11-15
                     "R U R' U' R U R'",
                     "R U R' U' R U R' U' R U R'",
                     "R U' R' U y' R' U R",
                     "y' R' U U R U R' U' R",
                     "y' U' R' U U R U' R' U R",
                     //16-20
                     "y' R' U R U' U' R' U' R",
                     "F U R U' R' F' R U' R'",
                     "U R U' R' U' R U' R' U R U' R'",
                     "R U' R' U U R U R'",
                     "U R U' U' R' U R U' R'",
                     //21-25
                     "R U' U' R' U' R U R'",
                     "U' R U' R' U U R U' R'",
                     "U' R U R' y' U R' U' R",
                     "y' U R' U R U' R' U' R",
                     "y' R' U' R",
                     //26-30
                     "y' U R' U' R U' R' U' R",
                     "y' R U' U' R' R' U' R R U' R'",
                     "l U r U' r' U' l'",
                     "U' R U' U' R' U U R U' R'",
                     "U' R U R' U' R U' U' R'",
                     //31-35
                     "U R U' R'",
                     "U' R U' U' R' U R U R'",
                     "y' U R' U' R d' R U R'",
                     "y' U' R' U R",
                     "y' U R' U' R U' U' R' U R",
                     //36-41
                     "y' U R' U U R U' U' R' U R",
                     "R U' R' U U y' R' U' R",
                     "R U' R' U R U' R' U U R U' R'",
                     "U' R U R' U R U R'",
                     "R U R'",
                     "U' R U' R' U R U R'"
                    };
    //57个公式
    string[] OLL = {//1-5 
                    "R U' U' R' R' F R F' U U R' F R F'",
                    "F R U R' U' F' f R U R' U' f'",
                    "f R U R' U' f' U' F R U R' U' F'",
                    "f R U R' U' f' U F R U R' U' F'",
                    "r' U U R U R' U r", 
                    //6-10
                    "r U' U' R' U' R U' r'",
                    "r U R' U R U' U' r'",
                    "r' U' R U' R' U U r",
                    "R U R' U' R' F R R U R' U' F'",
                    "R U R' U R' F R F' R U' U' R'",
                    //11-15
                    "r' R R U R' U R U' U' R' U M'",
                    "M' R' U' R U' R' U U R U' M",
                    "f R U R R U' R' U R U' f'",
                    "R' F R U R' F' R F U' F'",
                    "r' U' r R' U' R U r' U r",
                    //16-20
                    "r U r' R U R' U' r U' r'",
                    "l U' l' f R R B R' U R' U' f'",
                    "r U R' U R U' U' r' r' U' R U' R' U U r",
                    "r' R U R U R' U' r R' R' F R F'",
                    "r U R' U' M' M' U R U' R' U' M'",
                    //21-25
                    "R U' U' R' U' R U R' U' R U' R'",
                    "R U' U' R' R' U' R R U' R' R' U' U' R",
                    "R R D' R U' U' R' D R U' U' R",
                    "r U R' U' r' F R F'",
                    "F' r U R' U' r' F R",
                    //26-30
                    "R U' U' R' U' R U' R'",
                    "R U R' U R U' U' R'",
                    "r U R' U' r' R U R U' R'",
                    "R U R' U' R U' R' F' U' F R U R'",
                    "f R U R R U' R' U R R U' R' f'",
                    //31-35
                    "r' F' U F r U' r' U' r",
                    "R U B' U' R' U R B R'",
                    "R U R' U' R' F R F'",
                    "R U R R U' R' F R U R U' F'",
                    "R U' U' R' R' F R F' R U' U' R'",
                    //36-40
                    "R' U' R U' R' U R U l U' R' U x",
                    "F R U' R' U' R U R' F'",
                    "R U R' U R U' R' U' R' F R F'",
                    "R U R' F' U' F U R U U R'",
                    "R' F R U R' U' F' U R",
                    //41-45
                    "R U R' U R U' U' R' F R U R' U' F'",
                    "R' U' R U' R' U U R F R U R' U' F'",
                    "B' U' R' U R B",
                    "f R U R' U' f'",
                    "F R U R' U' F'",
                    //46-50
                    "R' U' R' F R F' U R",
                    "b' U' R' U R U' R' U R b",
                    "F R U R' U' R U R' U' F'",
                    "R B' R' R' F R R B R' R' F' R",
                    "r' U r r U' r' r' U' r r U r'",
                    //51-55
                    "f R U R' U' R U R' U' f'",
                    "R' F' U' F U' R U R' U R",
                    "r' U U R U R' U' R U R' U r",
                    "r U' U' R' U' R U R' U' R U' r'",
                    "r U' U' R' U' r' R R U R' U' r U' r'",
                    //56-57
                    "r U r' U R U' R' U R U' R' r U' r'",
                    "R U R' U' M' U R U' r'"
                    };
    //21个公式
    string[] PLL = {//1-5
                    "M' M' U M U U M' U M' M'",
                    "M' M' U' M U U M' U' M' M'",
                    "M' M' U M' M' U U M' M' U M' M'",
                    "M' U M' M' U M' M' U M' U U M' M' U'",
                    "x' R R D D R' U' R D D R' U R' x",
                    //6-10
                    "x' R U' R D D R' U R D D R' R' x",
                    "x' R U' R' D R U R' D' R U R' D R U' R' D' x",
                    "R U R' U' R' F R R U' R' U' R U R' F'",
                    "R' U' F' R U R' U' R' F R R U' R' U' R U R' U R",
                    "R' U R' d' R' F' R R U' R' U R' F R F",
                    //11-15
                    "F R U' R' U' R U R' F' R U R' U' R' F R F'",
                    "z U' R D' R R U R' U' R R U D R' z'",
                    "R U R' F' R U R' U' R' F R R U' R' U'",
                    "R' U U R U' U' R' F R U R' U' R' F' R R U'",
                    "R U' R' U' R U R D R' U' R D' R' U U R' U'",
                    //16-21
                    "R' R' u' R U' R U R' u R R f R' f'",
                    "R U R' y' R R u' R U' R' U R' u R R",
                    "R R u R' U R' U' R u' R' R' F' U F",
                    "R' d' F R R u R' U R U' R u' R' R'",
                    "R' U R U' R' F' U' F R U R' F R' F' R U' R",
                    "R U R' U R U R' F' R U R' U' R' F R R U' R' U U R U' R'"
                    };


    //21个PLL对应的情况
    string[] PLLBool =
    {
        //1-5
        "L红F绿:L绿:B蓝L红:B蓝:R橙B蓝:R红:F绿R橙:F橙",
        "L红F绿:L橙:B蓝L红:B蓝:R橙B蓝:R绿:F绿R橙:F红",
        "L红F绿:L橙:B蓝L红:B绿:R橙B蓝:R红:F绿R橙:F蓝",
        "L红F绿:L绿:B蓝L红:B橙:R橙B蓝:R蓝:F绿R橙:F红",
        "L橙F蓝:L红:B蓝L红:B蓝:R绿B橙:R橙:F红R绿:F绿",
        //6-10
        "L绿F橙:L红:B蓝L红:B蓝:R红B绿:R橙:F橙R蓝:F绿",
        "L蓝F红:L红:B红L绿:B蓝:R绿B橙:R橙:F橙R蓝:F绿",
        "L红F绿:L橙:B蓝L红:B蓝:R绿B橙:R红:F橙R蓝:F绿",
        "L红F绿:L红:B蓝L红:B绿:R绿B橙:R橙:F橙R蓝:F蓝",
        "L红F绿:L红:B绿L橙:B橙:R橙B蓝:R蓝:F蓝R红:F绿",
        //11-15
        "L红F绿:L蓝:B绿L橙:B红:R橙B蓝:R橙:F蓝R红:F绿",
        "L绿F橙:L红:B蓝L红:B蓝:R橙B蓝:R绿:F红R绿:F橙",
        "L红F绿:L红:B蓝L红:B蓝:R绿B橙:R绿:F橙R蓝:F橙",
        "L红F绿:L红:B橙L蓝:B蓝:R蓝B红:R绿:F绿R橙:F橙",
        "L红F绿:L蓝:B蓝L红:B红:R绿B橙:R橙:F橙R蓝:F绿",
        //16-21
        "L红F绿:L绿:B蓝L红:B橙:R绿B橙:R红:F橙R蓝:F蓝",
        "L红F绿:L橙:B蓝L红:B绿:R绿B橙:R蓝:F橙R蓝:F红",
        "L红F绿:L蓝:B蓝L红:B绿:R绿B橙:R红:F橙R蓝:F橙",
        "L红F绿:L橙:B蓝L红:B红:R绿B橙:R绿:F橙R蓝:F蓝",
        "L红F绿:L橙:B绿L橙:B蓝:R橙B蓝:R红:F蓝R红:F绿",
        "L橙F蓝:L橙:B蓝L红:B蓝:R红B绿:R红:F绿R橙:F绿"
    };
    //57个OLL对应的情况
    string[] OLLBool = { 
        //1-5
        "L黄:L黄:L黄:B黄:R黄:R黄:R黄:F黄",
        "L黄:L黄:L黄:B黄:B黄:R黄:F黄:F黄",
        "L黄:L黄:B黄:B黄:R黄:R黄:U黄:F黄",
        "F黄:L黄:L黄:B黄:U黄:R黄:R黄:F黄",
        "L黄:L黄:B黄:B黄:R黄:U黄:U黄:U黄",
        //6-10
        "F黄:L黄:L黄:U黄:U黄:U黄:R黄:F黄",
        "U黄:U黄:B黄:U黄:R黄:R黄:F黄:F黄",
        "F黄:U黄:U黄:B黄:B黄:R黄:R黄:U黄",
        "F黄:U黄:L黄:U黄:B黄:R黄:U黄:F黄",
        "L黄:U黄:B黄:B黄:U黄:R黄:F黄:U黄",
        //11-15
        "U黄:L黄:B黄:B黄:R黄:U黄:F黄:U黄",
        "F黄:L黄:U黄:U黄:B黄:U黄:R黄:F黄",
        "L黄:U黄:B黄:B黄:U黄:U黄:F黄:F黄",
        "F黄:U黄:L黄:B黄:B黄:U黄:U黄:F黄",
        "L黄:U黄:B黄:B黄:R黄:U黄:U黄:F黄",
        //16-20
        "F黄:U黄:L黄:B黄:U黄:U黄:R黄:F黄",
        "L黄:L黄:U黄:B黄:B黄:R黄:U黄:F黄",
        "F黄:L黄:U黄:B黄:U黄:R黄:F黄:F黄",
        "L黄:L黄:U黄:B黄:U黄:R黄:R黄:F黄",
        "U黄:L黄:U黄:B黄:U黄:R黄:U黄:F黄",
        //21-25
        "F黄:U黄:B黄:U黄:B黄:U黄:F黄:U黄",
        "L黄:U黄:L黄:U黄:B黄:U黄:F黄:U黄",
        "U黄:U黄:B黄:U黄:B黄:U黄:U黄:U黄",
        "F黄:U黄:B黄:U黄:U黄:U黄:U黄:U黄",
        "U黄:U黄:L黄:U黄:U黄:U黄:F黄:U黄",
        //26-30
        "F黄:U黄:L黄:U黄:U黄:U黄:R黄:U黄",
        "U黄:U黄:B黄:U黄:R黄:U黄:F黄:U黄",
        "U黄:U黄:U黄:U黄:U黄:R黄:U黄:F黄",
        "F黄:U黄:B黄:U黄:U黄:R黄:U黄:F黄",
        "L黄:L黄:U黄:B黄:U黄:U黄:R黄:U黄",
        //31-35
        "U黄:U黄:U黄:B黄:B黄:R黄:F黄:U黄",
        "F黄:L黄:B黄:B黄:U黄:U黄:U黄:U黄",
        "F黄:U黄:B黄:B黄:U黄:U黄:U黄:F黄",
        "U黄:U黄:L黄:B黄:R黄:U黄:U黄:F黄",
        "F黄:L黄:U黄:B黄:R黄:U黄:U黄:U黄",
       //36-40
        "F黄:U黄:U黄:B黄:R黄:R黄:U黄:U黄",
        "F黄:U黄:U黄:U黄:R黄:R黄:U黄:F黄",
        "U黄:U黄:B黄:U黄:U黄:R黄:R黄:F黄",
        "U黄:U黄:L黄:B黄:U黄:U黄:F黄:F黄",
        "L黄:U黄:U黄:B黄:B黄:U黄:U黄:F黄",
        //41-45
        "U黄:U黄:B黄:U黄:B黄:R黄:U黄:F黄",
        "F黄:U黄:U黄:B黄:U黄:R黄:F黄:U黄",
        "U黄:U黄:U黄:B黄:R黄:R黄:R黄:U黄",
        "L黄:L黄:L黄:B黄:U黄:U黄:U黄:U黄",
        "L黄:U黄:L黄:B黄:U黄:U黄:U黄:F黄",
        //46-50
        "U黄:L黄:U黄:U黄:R黄:R黄:R黄:U黄",
        "F黄:L黄:B黄:U黄:R黄:U黄:R黄:F黄",
        "L黄:U黄:L黄:U黄:B黄:R黄:F黄:F黄",
        "F黄:U黄:B黄:B黄:R黄:R黄:R黄:U黄",
        "L黄:L黄:L黄:B黄:B黄:U黄:F黄:U黄",
        //51-57
        "L黄:U黄:L黄:B黄:B黄:U黄:F黄:F黄",
        "L黄:L黄:L黄:U黄:B黄:R黄:F黄:U黄",
        "F黄:U黄:B黄:B黄:B黄:R黄:F黄:U黄",
        "F黄:U黄:B黄:U黄:B黄:R黄:F黄:F黄",
        "F黄:U黄:B黄:B黄:B黄:U黄:F黄:F黄",
        "L黄:U黄:L黄:B黄:R黄:U黄:R黄:F黄",
        "U黄:U黄:U黄:B黄:U黄:U黄:U黄:F黄"
    };
    //41个FLL角度对应的情况
    string[] F2LBool =
    {
        //"F蓝;R红;D白F蓝R红;R蓝F红;",
         //1-5
        "F:R:DFR:RF",
        "F:R:DFR:FU",
        "F:R:DFR:UR",
        "F:R:FRD:FR",
        "F:R:FRD:RF",
        //6-10
        "F:R:FRD:FU",
        "F:R:FRD:UR",
        "F:R:RDF:FR",
        "F:R:RDF:RF",
        "F:R:RDF:FU",
        //11-15
        "F:R:RDF:UR",
        "F:R:URF:FR",
        "F:R:URF:RF",
        "F:R:URF:FU",
        "F:R:URF:LU",
        //16-20
        "F:R:URF:BU",
        "F:R:URF:RU",
        "F:R:URF:UF",
        "F:R:URF:UL",
        "F:R:URF:UB",
        //21-25
        "F:R:URF:UR",
        "F:R:FUR:FR",
        "F:R:FUR:RF",
        "F:R:FUR:FU",
        "F:R:FUR:LU",
        //26-30
        "F:R:FUR:BU",
        "F:R:FUR:RU",
        "F:R:FUR:UF",
        "F:R:FUR:UL",
        "F:R:FUR:UB",
        //31-35
        "F:R:FUR:UR",
        "F:R:RFU:FR",
        "F:R:RFU:RF",
        "F:R:RFU:FU",
        "F:R:RFU:LU",
        //36-41
        "F:R:RFU:BU",
        "F:R:RFU:RU",
        "F:R:RFU:UF",
        "F:R:RFU:UL",
        "F:R:RFU:UB",
        "F:R:RFU:UR"
    };

    
    //图片资源
    Sprite[] PLLsprs;
    Sprite[] OLLsprs;
    Sprite[] F2Lsprs;
    Sprite empty;

    CubeStruct[] cubeStructs = new CubeStruct[27];

    void Start()
    {
        Instance = this;
        //获取图片资源
        PLLsprs = Resources.LoadAll<Sprite>("PLL");
        OLLsprs = Resources.LoadAll<Sprite>("OLL");
        F2Lsprs = Resources.LoadAll<Sprite>("F2L");
        empty = Resources.Load<Sprite>("Image");
        //找到小地图摄像机
        miCamera = GameObject.Find("Mini Camera").gameObject;
        //找到按钮界面
        btnCanvas = GameObject.Find("BtnCanvas").gameObject;
        buttons = new Button[btnName.Length];
        //按钮绑定
        for (int i = 0; i < btnName.Length; i++)
        {
            buttons[i] = transform.Find(btnName[i]).GetComponent<Button>();
            EventTriggerListener.Get(buttons[i].gameObject).onClick = OnButtonClick;
        }
    }
    private void OnButtonClick(GameObject go)
    {
        //在这里监听按钮的点击事件
        switch (go.name)
        {
            case "mstateMix"://魔方打乱
                if (setClick)
                    StartCoroutine("MixCube", 25);
                break;
            case "mstateSave"://魔方状态保存
                if (setClick)
                    SaveMofanSta();
                break;
            case "mstateRecovery"://魔方状态复原
                if (setClick)
                    ReseMofanSta();
                break;
            case "MofanRestore"://魔方底十字复原
                if (setClick && GameManager.Instance.RestoreCheck()=="")
                    StartCoroutine("mofanDRestore");
                break;
            case "MofanCheck"://魔方公式提示
                if (setClick)
                {
                    UIinit.Instance.ChangeValue("", empty);
                    CheckFan();
                }
                break;
        }
    }
    //打乱魔方
    IEnumerator MixCube(int count)
    {
        setClick = false;
        //按钮界面及小摄像机消失
        btnCanvas.SetActive(false);
        miCamera.SetActive(false);
        for (int i = 0; i < count; i++)
        {
            GameManager.Instance.InStrMofan(mxi_strs[Random.Range(0,14)],false);
            //等待0.23秒
            yield return new WaitForSeconds(0.23f);
        }
        //按钮界面及小摄像机重现
        miCamera.SetActive(true);
        btnCanvas.SetActive(true);
        //协程结束
        StopCoroutine("MixCube");
        setClick = true;
        CheckFan();
        yield return null;
    }
    //魔方底十字复原
    IEnumerator mofanDRestore()
    {
        setClick = false;
        //按钮界面消失
        btnCanvas.SetActive(false);
        string Restr = StartUP();
        if (Restr != "")
            yield return StartCoroutine(GameManager.Instance.InMStrMfan(Restr,false));
        //Cross还原
        while (RestoreNum() != 4)
        {
            //小黄花还原
            yield return StartCoroutine(CrossCenter());
            yield return StartCoroutine(CrossDown());
            yield return StartCoroutine(CrossDownUP());
        }
        //白色翻面
        yield return StartCoroutine(reWhite());
        //协程结束
        StopCoroutine("mofanDRestore");
        btnCanvas.SetActive(true);
        setClick = true;
        CheckFan();
        yield return null;
    }

    //切换为F2L检测
    public void switchFormula(int n)
    {
        if (GameManager.Instance.RestoreCheck() == "白色十字架")
        {
            reChoose += n;
            if (reChoose == 4)
                reChoose = 0;
            if (reChoose == -1)
                reChoose = 3;
            UIinit.Instance.ChangeValue("", empty);
            CheckFan();
        }
    }
    //检测并更新魔方公式
    public void CheckFan()
    {
        switch (GameManager.Instance.RestoreCheck())
        {
            case "魔方已复原":
                GameManager.Instance.setMofanView("魔方已复原");
                UIinit.Instance.ChangeValue("魔方已复原", "无", "", empty);
                break;
            case ""://魔方初始混乱状态
                GameManager.Instance.setMofanView("白色十字架");
                UIinit.Instance.ChangeValue("Cross", "底部白色十字架", "", empty);
                break;
            case "白色十字架"://白色十字架已完成
                GameManager.Instance.setMofanView("白色底两层");
                UIinit.Instance.ChangeValue("F2L", f2lColour[reChoose]);
                F2LRestore(readF2LArr(reChoose));
                break;
            case "白色底两层"://白色底两层已完成
                GameManager.Instance.setMofanView("白色底两层,黄色顶层");
                UIinit.Instance.ChangeValue("OLL", "黄色顶层翻色");
                POLLRestore(true);
                break;
            case "白色底两层,黄色顶层"://白色底两层和黄色顶层翻色已完成
                GameManager.Instance.setMofanView("魔方已复原");
                UIinit.Instance.ChangeValue("PLL", "黄色顶层顺序调整");
                POLLRestore(false);
                break;
        }
    }

    #region Cross
    //开始让黄色中心块在U面
    string StartUP()
    {
        string CrossStr = "";
        switch (MofanRestore.Instance.MofanStata("M13"))
        {
            case "U白":
                CrossStr = "M M";
                break;
            case "F白":
                CrossStr = "M";
                break;
            case "B白":
                CrossStr = "M'";
                break;
            case "R白":
                CrossStr = "S";
                break;
            case "L白":
                CrossStr = "S'";
                break;
        }
        return CrossStr;
    }
    //最后黄色层转U
    string LastUP()
    {
        string str = MofanRestore.Instance.readMofanFixed("M11:M12", true);
        char c1 = str[0];
        char c2 = str[5];
        int n = 0;
        while (c1 != c2 &&n<3)
        {
            switch (c1)
            {
                case 'F':
                    c1 = 'L';
                    break;
                case 'L':
                    c1 = 'B';
                    break;
                case 'B':
                    c1 = 'R';
                    break;
                case 'R':
                    c1 = 'F';
                    break;
            }
            n++;
        }

        switch (n)
        {
            case 1:
                UIinit.Instance.ChangeValue("U'", empty);
                return "U'";
            case 2:
                UIinit.Instance.ChangeValue("U U", empty);
                return "U U";
            case 3:
                UIinit.Instance.ChangeValue("U", empty);
                return "U";
        }
        return "";
    }

    //白色翻转
    IEnumerator reWhite()
    {
        string[] arrs = readCross(true);
        string[] cents = readCenter(true);
        for(int i = 0; i < arrs.Length; i++)
        {
            if (arrs[i][0] == 'U')
            {
                while (arrs[i].Substring(2)!=cents[i])
                {
                    yield return StartCoroutine(GameManager.Instance.InMStrMfan("U", false));
                    arrs = readCross(true);
                    cents = readCenter(true);
                }
                yield return StartCoroutine(GameManager.Instance.InMStrMfan(cents[i][0]+" "+ cents[i][0],false));
            }
        }
        yield return null;
    }
    //将方向不正确的底层和顶层转成中间层
    IEnumerator CrossDownUP()
    {
        int tamp = 0;
        string[] arrs;
        int n = 0;
        //还原底层
        while (isDownUpCube())
        {
            arrs = readCross(false);
            n = n % 4;
            if (arrs[n][1] == 'U' || arrs[n][1] == 'D')
            {
                //判断转上去是否有阻碍，最多三次
                tamp = 0;
                while (isContainsU(arrs[n][0]) && tamp < 3)
                {
                    yield return StartCoroutine(GameManager.Instance.InMStrMfan("U",false));
                    tamp++;
                }
                yield return StartCoroutine(GameManager.Instance.InMStrMfan(arrs[n][0] + "",false));
            }
            n++;
        }
        yield return null;
    }
    //还原底层且方向正确的
    IEnumerator CrossDown()
    {
        int tamp = 0;
        string[] arrs;
        int n = 0;
        //还原底层
        while (isDownCube())
        {
            arrs = readCross(false);
            n = n % 4;
            if (arrs[n][0] == 'D')
            {
                //判断转上去是否有阻碍，最多三次
                tamp = 0;
                while (isContainsU(arrs[n][1]) && tamp < 3)
                {
                    yield return StartCoroutine(GameManager.Instance.InMStrMfan("U",false));
                    tamp++;
                }
                yield return StartCoroutine(GameManager.Instance.InMStrMfan(arrs[n][1] + " "+ arrs[n][1],false));
            }
            n++;
        }
        yield return null;
    }
    //还原中间层
    IEnumerator CrossCenter()
    {
        int tamp = 0;
        string[] arrs;
        int n = 0;
        
        while (isCenCube())
        {
            arrs = readCross(false);
            n = n % 4;
            if (arrs[n][0] != 'U' && arrs[n][0] != 'D' && arrs[n][1] != 'U' && arrs[n][1] != 'D')
            {
                //判断转上去是否有阻碍，最多三次
                tamp = 0;
                while (isContainsU(arrs[n][1]) && tamp < 3)
                {
                    yield return StartCoroutine(GameManager.Instance.InMStrMfan("U",false));
                    tamp++;
                }
                switch (isCenterU(arrs[n]))
                {
                    case 0:
                        yield return StartCoroutine(GameManager.Instance.InMStrMfan(arrs[n][1] + "'",false));
                        break;
                    case 1:
                        yield return StartCoroutine(GameManager.Instance.InMStrMfan(arrs[n][1] + "",false));
                        break;
                    case -1:
                        Debug.Log("Aka");
                        break;
                }
            }
            n++;
        }
        yield return null;
    }
    //顶部还原个数
    int RestoreNum()
    {
        string[] arrs = readCross(false);
        int count = 0;
        for (int i = 0; i < arrs.Length; i++)
        {
            //在已还原中去找
            if (arrs[i][0] == 'U')
            {
                count++;
            }
        }
        return count;
    }

    //底部和顶部是否有不正确方块
    bool isDownUpCube()
    {
        string[] arrs = readCross(false);
        for (int i = 0; i < arrs.Length; i++)
        {
            if (arrs[i][1] == 'U' || arrs[i][1] == 'D')
            {
                return true;

            }
        }
        return false;
    }
    //底部是否有正确方块
    bool isDownCube()
    {
        string[] arrs = readCross(false);
        for (int i = 0; i < arrs.Length; i++)
        {
            if (arrs[i][0] == 'D')
            {
                return true;

            }
        }
        return false;
    }
    //中间是否还有方块
    bool isCenCube()
    {
        string[] arrs = readCross(false);
        for (int i = 0; i < arrs.Length; i++)
        {
            if (arrs[i][0] != 'U' && arrs[i][0] != 'D' && arrs[i][1] != 'U' && arrs[i][1] != 'D')
            {
                return true;
            }
        }
        return false;
    }
    //中间方块的方向
    int isCenterU(string s)
    {
        switch (s)
        {
            case "FR":
            case "LF":
            case "BL":
            case "RB":
                return 1;
            case "RF":
            case "FL":
            case "LB":
            case "BR":
                return 0;
        }
        return -1;
    }

    //是否有与c相同的字符，有则true
    bool isContainsU(char c)
    {
        string[] arrs = readCross(false);
        for (int i = 0; i < arrs.Length; i++)
        {
            //在已还原中去找
            if(arrs[i][0] == 'U')
            {
                if (arrs[i][1] == c)
                    return true;
            }
        }
        return false;
    }
    //读取中间方块
    string[] readCenter(bool isHaveColour)
    {
        string str = MofanRestore.Instance.readMofanFixed("M5:M17:M23:M11", isHaveColour);
        return str.Split(':');
    }
    //读取Cross字符
    string[] readCross(bool isHaveColour)
    {
        //                                              白红 白蓝 白橙 白绿
        string str = MofanRestore.Instance.readMofanFixed("M4:M16:M22:M10", isHaveColour);
        return str.Split(':');
    }
    #endregion
    #region F2L
    /// <summary>
    /// 读取的魔方F2L情况，判定魔方块的分布情况
    /// </summary>
    /// <param name="str">读取的字符串</param>
    /// <returns>返回方块分布情况，0/1/2/3属于,4/5/6/7不属于,-1错误</returns>
    int F2LRange(string str)
    {
        //分割成数组，便于对比，str格式为F:R:RUB:FR
        string[] arrs = str.Split(':');
        int reCode = -1;

        char[] charArr = str.ToCharArray();
        string strResult = "";
        //判定有多少个块在第三层
        for (int i = 4; i < charArr.Length; i++)
        {
            if (charArr[i] == 'U')
            {
                switch (i)
                {
                    //角块
                    case 4:
                    case 5:
                    case 6:
                        strResult += "角块;";
                        break;
                    //棱块
                    case 8:
                    case 9:
                        strResult += "棱块;";
                        break;
                }
            }
        }
        //返回方块分布情况代码
        //0/1/2/3属于,4/5/6/7不属于,-1错误
        switch (strResult)
        {
            case "":
                if (arrs[2][1] + "" == arrs[0] && arrs[2][2] + "" == arrs[1] && arrs[3][0] + "" == arrs[0] && arrs[3][1] + "" == arrs[1])
                {
                    UIinit.Instance.ChangeValue("已还原",empty);
                }
                //位置对两个
                else if ((arrs[2].Contains(arrs[0]) && arrs[2].Contains(arrs[1])) && (arrs[3].Contains(arrs[0]) && arrs[3].Contains(arrs[1])))
                {
                    reCode = 0;
                }
                //角块对
                else if (arrs[2].Contains(arrs[0]) && arrs[2].Contains(arrs[1]))
                {
                    unF2LNoU(arrs, false);
                }
                //棱块对
                else if (arrs[3].Contains(arrs[0]) && arrs[3].Contains(arrs[1]))
                {
                    unF2LNoU(arrs, true);
                }
                //都不对
                else
                {
                    unF2LNoU(arrs, false);
                }
                break;
            case "角块;":
                if (arrs[3].Contains(arrs[0]) && arrs[3].Contains(arrs[1]))
                    reCode = 1;
                else
                    unF2LStr(arrs, false);
                break;
            case "棱块;":
                if (arrs[2].Contains(arrs[0]) && arrs[2].Contains(arrs[1]))
                    reCode = 2;
                else
                    unF2LStr(arrs,true);
                break;
            case "角块;棱块;":
                reCode = 3;
                break;
        }
        return reCode;
    }
    //有U判断的非标F2L转标F2L
    void unF2LStr(string[] arrs,bool isjiao)
    {
        string s=isjiao? arrs[2].Replace("D", "") : arrs[3];
        int k = isCenterU(s);
        if (k == -1)
            return;
        if (isjiao? !arrs[3].Contains(s[1 ^ k] + "") : arrs[2].Contains(arrs[3][0] + "") && arrs[2].Contains(arrs[3][1] + ""))
        {
            UIinit.Instance.ChangeValue(s[k] + " U' " + s[k] + "'",empty);
        }
        else
        {
            UIinit.Instance.ChangeValue(s[k] + " U " + s[k] + "'",empty);
        }
    }
    //无U判断的非标F2L转标F2L
    void unF2LNoU(string[] arrs, bool isjiao)
    {
        string s = isjiao ? arrs[2].Replace("D", "") : arrs[3];
        int k = isCenterU(s);
        if (k != -1)
            UIinit.Instance.ChangeValue(s[k] + " U " + s[k] + "'", empty);
    }

    //将读取的魔方F2L情况，看是否属于F2L公式范围，若是则模拟魔方转动来返回匹配F2L的公式
    string F2LRestore(string str)
    {
        int numType = F2LRange(str);
        if (numType == -1)
        {
            return "不属于F2L公式范围";
        }
        int swapCount = 0;
        string numStr = "";

        for (int i = 0; i < F2LBool.Length; i++)
        {
            swapCount = 0;
            while (F2LUNum(F2LBool[i], str, numType, ref numStr) && swapCount < 4)
            {
                reStr(ref str);
                swapCount++;
            }
            //swapCount,0,1,2,3是找到了，4对应的是没找到
            switch (swapCount)
            {
                case 0:
                    UIinit.Instance.ChangeValue(""+ numStr, F2Lsprs[i]);
                    return numStr + F2L[i];
                case 1:
                    UIinit.Instance.ChangeValue("y "+ numStr, F2Lsprs[i]);
                    return "y " + numStr + F2L[i];
                case 2:
                    UIinit.Instance.ChangeValue("y y "+ numStr, F2Lsprs[i]);
                    return "y y " + numStr + F2L[i];
                case 3:
                    UIinit.Instance.ChangeValue("y' "+ numStr, F2Lsprs[i]);
                    return "y' " + numStr + F2L[i];
            }
        }
        return "F2L未找到";
    }
    /// <summary>
    /// 属于F2L公式范围,然后判定是否有U要转动
    /// </summary>
    /// <param name="s1">以记录的F2L的情况</param>
    /// <param name="s2">实时读取的F2L字符串</param>
    /// <param name="numType">F2L所属情况</param>
    /// <param name="numStr">反馈的U要转动的值</param>
    /// <returns>是否找到了要转动U的情况</returns>
    bool F2LUNum(string s1, string s2, int numType, ref string numStr)
    {
        int count = 0;
        while (s1 != s2 && count < 4)
        {
            reStrF2L(ref s2, numType);
            count++;
        }
        //0,1,2,3是找到了，4对应的是没找到
        switch (count)
        {
            case 1:
                numStr = "U ";
                break;
            case 2:
                numStr = "U U ";
                break;
            case 3:
                numStr = "U' ";
                break;
            case 4:
                return true;
        }
        return false;
    }
    /// <summary>
    /// F2L的指定字符进行替换
    /// </summary>
    /// <param name="str">传入要替换的字符</param>
    /// <param name="numType">F2L所属情况</param>
    void reStrF2L(ref string str, int numType)
    {
        //分割成数组，便于对比
        string[] arrs = str.Split(':');
        switch (numType)
        {
            case 0:
                return;
            case 1:
                reStr(ref arrs[2]);
                break;
            case 2:
                reStr(ref arrs[3]);
                break;
            case 3:
                reStr(ref arrs[2]);
                reStr(ref arrs[3]);
                break;
        }
        str = string.Join(":", arrs);
    }
    /// <summary>
    /// 读取F2L的块
    /// </summary>
    /// <returns>红绿;绿橙;橙蓝;蓝红</returns>
    string readF2LArr(int n)
    {
        string[] F2LArr = { "M5:M11:M1:M2", "M11:M23:M19:M20", "M23:M17:M25:M26", "M17:M5:M7:M8" };
        return MofanRestore.Instance.readMofanFixed(F2LArr[n], false);
    }
    #endregion
    #region PLL
    /// <summary>
    /// 读取的魔方PLL/OLL情况，模拟魔方转动来返回匹配的公式
    /// </summary>
    /// <param name="str">实时读取的PLL/OLL字符</param>
    /// <param name="isOLL">是否为OLL</param>
    /// <returns>返回匹配出的PLL/OLL公式</returns>
    string POLLRestore(bool isOLL)
    {
        string[] arrs = readPOLLArr();
        //去除多余数据
        removeExcessStr(ref arrs, isOLL);
        //记录下交换对比次数
        int swapCount = 0;
        //PLL/OLL的对比字符集
        string[] ContrastStr = { };
        //PLL/OLL的还原字符集
        string[] RecoverStr = { };
        //PLL/OLL的还原Sprite集
        Sprite[] RecoverSpr = { };
        if (isOLL)
        {
            ContrastStr = OLLBool;
            RecoverSpr = OLLsprs;
            RecoverStr = OLL;
        }
        else
        {
            ContrastStr = PLLBool;
            RecoverSpr = PLLsprs;
            RecoverStr = PLL;
        }
        for (int i = 0; i < ContrastStr.Length; i++)
        {
            swapCount = 0;
            while (POLLTypeCom(ContrastStr[i], string.Join(":", arrs), isOLL) && swapCount < 4)
            {
                POLLSWapReStr(ref arrs);
                swapCount++;
            }
            //swapCount,0,1,2,3是找到了，4对应的是没找到
            switch (swapCount)
            {
                case 0:
                    UIinit.Instance.ChangeValue("", RecoverSpr[i]);
                    return RecoverStr[i];
                case 1:
                    UIinit.Instance.ChangeValue("y", RecoverSpr[i]);
                    return "y " + RecoverStr[i];
                case 2:
                    UIinit.Instance.ChangeValue("y y", RecoverSpr[i]);
                    return "y y " + RecoverStr[i];
                case 3:
                    UIinit.Instance.ChangeValue("y'", RecoverSpr[i]);
                    return "y' " + RecoverStr[i];
            }
        }
        if (isOLL)
            return "OLL未找到";
        return "PLL未找到";
    }
    //读取PLL/OLL字符数组
    string[] readPOLLArr()
    {
        string str = MofanRestore.Instance.readMofanOrder("M3:M6:M9:M18:M27:M24:M21:M12");
        return str.Split(':');
    }
    //去除多余数据
    void removeExcessStr(ref string[] arrs,bool isOLL)
    {
        for (int i = 0; i < arrs.Length; i++)
        {
            if (isOLL)
            {
                arrs[i] = arrs[i].Substring(0, 2);
            }
            else
            {
                arrs[i] = arrs[i].Replace("U黄", "");
            }

        }
    }
    //模拟魔方转动，顺序改变及字符替换，得到转动后的字符
    void POLLSWapReStr(ref string[] arrs)
    {
        int n = 0;
        string[] tams = new string[arrs.Length];
        //字符位置顺序调转
        for (int i = 0; i < arrs.Length; i++)
        {
            n = (i + 6) % (arrs.Length);
            tams[i] = arrs[n];
            //“F/L/B/R”字符替换
            reStr(ref tams[i]);
        }
        arrs = tams;
    }
    /// <summary>
    /// PLL/OLL类型对比,OLL一次,PLL比较四次,主比较PLL，看类型是否相同
    /// </summary>
    /// <param name="s1">固定的PLL/OLL字符集</param>
    /// <param name="s2">动态的读取的字符</param>
    /// <param name="isOLL">是否为OLL情况</param>
    /// <returns>false则为PLL类型相同</returns>
    bool POLLTypeCom(string s1,string s2,bool isOLL)
    {
        //OLL比较一次
        if (isOLL)
            return s1 != s2;
        //PLL比较四次
        int count = 0;
        while (s1 != s2 && count < 4)
        {
            reStrColour(ref s2);
            count++;
        }
        if (count != 4)
            return false;
        return true;
    }
    #endregion

    //颜色字符替换
    void reStrColour(ref string s)
    {
        char[] charArr = s.ToCharArray();
        s = "";
        for (int j = 0; j < charArr.Length; j++)
        {
            switch (charArr[j])
            {
                case '红':
                    charArr[j] = '绿';
                    break;
                case '绿':
                    charArr[j] = '橙';
                    break;
                case '橙':
                    charArr[j] = '蓝';
                    break;
                case '蓝':
                    charArr[j] = '红';
                    break;
            }
            s += charArr[j];
        }
    }
    //将string转为char,进行F/L/B/R的顺时针替换
    void reStr(ref string s)
    {
        char[] charArr = s.ToCharArray();
        s = "";
        for (int j = 0; j < charArr.Length; j++)
        {
            switch (charArr[j])
            {
                case 'F':
                    charArr[j] = 'L';
                    break;
                case 'L':
                    charArr[j] = 'B';
                    break;
                case 'B':
                    charArr[j] = 'R';
                    break;
                case 'R':
                    charArr[j] = 'F';
                    break;
            }
            s += charArr[j];
        }
    }

    //保存魔方状态
    void SaveMofanSta()
    {
        for (int i = 0; i < GameManager.Instance.cubePrefabList.Length; i++)
        {
            cubeStructs[i].mofanPos = GameManager.Instance.cubePrefabList[i].transform.position;
            cubeStructs[i].mofanRot = GameManager.Instance.cubePrefabList[i].transform.eulerAngles;
        }
    }
    //还原魔方状态
    void ReseMofanSta()
    {
        for (int i = 0; i < GameManager.Instance.cubePrefabList.Length; i++)
        {
            if (cubeStructs[0].mofanPos == Vector3.zero)
                break;
            GameManager.Instance.cubePrefabList[i].transform.position = cubeStructs[i].mofanPos;
            GameManager.Instance.cubePrefabList[i].transform.eulerAngles = cubeStructs[i].mofanRot;
        }
        //魔方的控制器类型重置
        GameManager.pre_mofantype = "";
        GameObject.Find("Main Camera").SendMessage("InFAngles");
    }
}
