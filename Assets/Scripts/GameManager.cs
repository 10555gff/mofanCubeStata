using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public GameObject[] cubePrefabList,cubeViewPrefabList,mofanControlList;
    public Material[] myMaterialList;
    GameObject mofan;
    //魔方的方块是否加入
    bool isadd = false;
    //魔方的控制器编号
    int mofanConNum = -1;
    //魔方的旋转角度
    Vector3 mofan_rotate;
    //记录魔方的类型
    public static string pre_mofantype = "";
    //记录魔方的角度值
    int[] mofan_arr = new int[12];
    //魔方是否转动完毕
    public bool isFinish=true;
    //暂存魔方的角度值
    int n1 =0;
    //魔方的输入公式字符
    char var_ch;
    //魔方位置是否正确
    bool isP;
    //存魔方阶段
    List<string> mofanStageList = new List<string>();
    //接收协程的返回值
    public Coroutine stopCor;

    public enum Axis
    {
        //魔方的轴
        X ,Y ,Z,O
    }

    void Start()
    {
        Instance = this;
        //魔方的主体
        mofan = GameObject.Find("mofanCreate");
        //魔方的12个控制器
        mofanControlList = GameObject.FindGameObjectsWithTag("mofan_control");
        //魔方的27个方块
        cubePrefabList = GameObject.FindGameObjectsWithTag("mofan_cube");
        //魔方的27个观看方块
        cubeViewPrefabList = GameObject.FindGameObjectsWithTag("mofan_view");
        setMofanView("魔方已复原");
    }
    //设置观察魔方的材质
    public void setMofanView(string str)
    {
        switch (str)
        {
            case "魔方已复原":
                for (int i = 0; i < cubeViewPrefabList.Length; i++)
                {
                    cubeViewPrefabList[i].GetComponentInChildren<Renderer>().material = myMaterialList[0];
                }
                break;
            case "白色底两层,黄色顶层":
                for (int i = 0; i < cubeViewPrefabList.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 3:
                        case 4:
                        case 6:
                        case 7:
                        case 9:
                        case 10:
                        case 12:
                        case 14:
                        case 15:
                        case 16:
                        case 18:
                        case 19:
                        case 21:
                        case 22:
                        case 24:
                        case 25:
                            cubeViewPrefabList[i].GetComponentInChildren<Renderer>().material = myMaterialList[0];
                            break;
                        case 2:
                        case 5:
                        case 8:
                        case 11:
                        case 17:
                        case 20:
                        case 23:
                        case 26:
                            cubeViewPrefabList[i].GetComponentInChildren<Renderer>().material = myMaterialList[3];
                            break;
                    }
                }
                break;
            case "白色底两层":
                for (int i = 0; i < cubeViewPrefabList.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 3:
                        case 4:
                        case 6:
                        case 7:
                        case 9:
                        case 10:
                        case 12:
                        case 14:
                        case 15:
                        case 16:
                        case 18:
                        case 19:
                        case 21:
                        case 22:
                        case 24:
                        case 25:
                            cubeViewPrefabList[i].GetComponentInChildren<Renderer>().material = myMaterialList[0];
                            break;
                        case 2:
                        case 5:
                        case 8:
                        case 11:
                        case 17:
                        case 20:
                        case 23:
                        case 26:
                            cubeViewPrefabList[i].GetComponentInChildren<Renderer>().material = myMaterialList[1];
                            break;
                    }
                }
                break;
            case "白色十字架":
                for (int i = 0; i < cubeViewPrefabList.Length; i++)
                {
                    switch (i)
                    {
                        case 3:
                        case 4:
                        case 9:
                        case 10:
                        case 12:
                        case 14:
                        case 15:
                        case 16:
                        case 21:
                        case 22:
                            cubeViewPrefabList[i].GetComponentInChildren<Renderer>().material = myMaterialList[0];
                            break;
                        case 0:
                        case 1:
                        case 2:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 11:
                        case 13:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                            cubeViewPrefabList[i].GetComponentInChildren<Renderer>().material = myMaterialList[1];
                            break;
                    }
                }
                break;
        }
    }

    //魔方阶段状态检测
    public string RestoreCheck()
    {
        //检测每个魔方块，将复原的魔方块加入泛型数例
        for (int i = 0; i < cubePrefabList.Length; i++)
        {
            switch (cubePrefabList[i].name)
            {
                //中心块相对位置，不用检测
                case "M5":
                case "M11":
                case "M13":
                case "M14":
                case "M15":
                case "M17":
                case "M23":
                    isP = false;
                    break;

                //顶棱和底棱
                case "M6":
                case "M12":
                case "M18":
                case "M24":
                    isP = mofanPCheck(i, i - 1, 14);
                    break;
                case "M4":
                case "M10":
                case "M16":
                case "M22":
                    isP = mofanPCheck(i, i + 1, 12);
                    break;
                //侧棱块
                case "M2":
                    isP = mofanPCheck(i, 4, 10);
                    break;
                case "M8":
                    isP = mofanPCheck(i, 16, 4);
                    break;
                case "M20":
                    isP = mofanPCheck(i, 10, 22);
                    break;
                case "M26":
                    isP = mofanPCheck(i, 22, 16);
                    break;

                //顶角块
                case "M3":
                    isP = mofanPCheck(i, 4, 10, 14);
                    break;
                case "M9":
                    isP = mofanPCheck(i, 16, 4, 14);
                    break;
                case "M21":
                    isP = mofanPCheck(i, 10, 22, 14);
                    break;
                case "M27":
                    isP = mofanPCheck(i, 22, 16, 14);
                    break;

                //底角块
                case "M1":
                    isP = mofanPCheck(i, 4, 10, 12);
                    break;
                case "M7":
                    isP = mofanPCheck(i, 16, 4, 12);
                    break;
                case "M19":
                    isP = mofanPCheck(i, 10, 22, 12);
                    break;
                case "M25":
                    isP = mofanPCheck(i, 22, 16, 12);
                    break;

            }
            //位置加角度检测
            if (isP && mofanRCheck(i))
            {
                //将以复原的魔方块加入泛型数例
                mofanStageList.Add(cubePrefabList[i].name);
            }
        }

        string strReuslt = "";
        if (mofanStageList.Count == 20)
        {
            strReuslt = "魔方已复原";
        }
        //魔方底两层复原和顶层翻色
        else if (mofanStageList.Count>=12&&mofanContains("M2:M8:M20:M26:M1:M4:M7:M10:M16:M19:M22:M25"))
        {
            strReuslt = "白色底两层";
            if (mofanTopCheck("M15", "M3:M6:M9:M12:M18:M21:M24:M27"))
            {
                strReuslt += ",黄色顶层";
                if (mofanTop())
                    strReuslt += ",U未对";
            }
        }
        //魔方底层十字架复原
        else if (mofanContains("M4:M10:M16:M22"))
        {
            strReuslt = "白色十字架";
        }
        //泛型数例清空
        mofanStageList.Clear();
        return strReuslt;
    }
    //判断是否黄色翻色成功并且顺序对了，但U不对的情况
    bool mofanTop()
    {
        switch (MofanRestore.Instance.readMofanOrder("M3:M6:M9:M18:M27:M24:M21:M12"))
        {
            case "U黄L红F绿:U黄L红:U黄B蓝L红:U黄B蓝:U黄R橙B蓝:U黄R橙:U黄F绿R橙:U黄F绿":
            case "U黄L绿F橙:U黄L绿:U黄B红L绿:U黄B红:U黄R蓝B红:U黄R蓝:U黄F橙R蓝:U黄F橙":
            case "U黄L橙F蓝:U黄L橙:U黄B绿L橙:U黄B绿:U黄R红B绿:U黄R红:U黄F蓝R红:U黄F蓝":
            case "U黄L蓝F红:U黄L蓝:U黄B橙L蓝:U黄B橙:U黄R绿B橙:U黄R绿:U黄F红R绿:U黄F红":
                return true;
        }
        return false;
    }
    /// <summary>
    /// 魔方块是否全部翻色成功
    /// </summary>
    /// <param name="str">参考魔方块</param>
    /// <param name="content">参考物魔方块列表</param>
    /// <returns></returns>
    bool mofanTopCheck(string str, string content)
    {
        string[] strarr = content.Split(':');
        string first = MofanRestore.Instance.MofanStata(str);
        for (int i = 0; i < strarr.Length; i++)
        {
            if (!MofanRestore.Instance.MofanStata(strarr[i]).Contains(first))
            {
                return false;
            }
        }
        return true;
    }
    //是否包含这已还原的方块
    bool mofanContains(string str)
    {
        string[] strarr = str.Split(':');
        for(int i = 0; i < strarr.Length; i++)
        {
            //若有一个不包含的则返回false
            if (!mofanStageList.Contains(strarr[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 判断棱块位置是否正确
    /// </summary>
    /// <param name="n1">魔方的棱块</param>
    /// <param name="n2">魔方中心块1</param>
    /// <param name="n3">魔方中心块2</param>
    /// <returns>真或假</returns>
    bool mofanPCheck(int n1, int n2,int n3)
    {
        if(cubePrefabList[n1].transform.position.ToString()== (cubePrefabList[n2].transform.position + cubePrefabList[n3].transform.position).ToString())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 判断角块位置是否正确
    /// </summary>
    /// <param name="n1">魔方的棱块</param>
    /// <param name="n2">魔方中心块1</param>
    /// <param name="n3">魔方中心块2</param>
    /// <param name="n4">魔方中心块3</param>
    /// <returns>真或假</returns>
    bool mofanPCheck(int n1, int n2, int n3,int n4)
    {
        if (cubePrefabList[n1].transform.position.ToString() == (cubePrefabList[n2].transform.position + cubePrefabList[n3].transform.position + cubePrefabList[n4].transform.position).ToString())
        {
            return true;
        }
        return false;
    }
    //魔方角度是否正确
    bool mofanRCheck(int num)
    {
        //if (MofanRestore.Instance.rType(cubePrefabList[num].transform.rotation.ToString()) == MofanRestore.Instance.rType(cubePrefabList[13].transform.rotation.ToString()))
        //{
        //    return true;
        //}
        return true;
    }

    //魔方多公式输入
    public IEnumerator InMStrMfan(string str,bool isbool)
    {
        string[] strarry = str.Split(' ');
        for (int i = 0; i < strarry.Length; i++)
        {
            InStrMofan(strarry[i], isbool);
            //等待0.23秒
            yield return new WaitForSeconds(0.23f);
        }
        yield return null;
    }
    //魔方单公式输入
    public void InStrMofan(string t1,bool isbool)
    {
        //顺、逆时针旋转
        if(t1.Substring(t1.Length - 1, 1)=="'")
        {
            UIinit.btn_type = 2;
        }
        var_ch = t1.ToCharArray()[0];
        if (var_ch >= 'a' && var_ch <= 'z')
        {
            switch (var_ch)
            {
                case 'x':
                    mofanBtnOnclick("x", Axis.O, isbool);
                    break;
                case 'y':
                    mofanBtnOnclick("y", Axis.O, isbool);
                    break;
                case 'z':
                    mofanBtnOnclick("z", Axis.O, isbool);
                    break;
                case 'f':
                    mofanBtnOnclick("f", Axis.Z, isbool);
                    break;
                case 'b':
                    mofanBtnOnclick("b", Axis.Z, isbool);
                    break;
                case 'r':
                    mofanBtnOnclick("r", Axis.X, isbool);
                    break;
                case 'l':
                    mofanBtnOnclick("l", Axis.X, isbool);
                    break;
                case 'u':
                    mofanBtnOnclick("u", Axis.Y, isbool);
                    break;
                case 'd':
                    mofanBtnOnclick("d", Axis.Y, isbool);
                    break;
            }
        }
        else if (var_ch >= 'A' && var_ch <= 'Z')
        {
            switch (var_ch)
            {
                case 'E':
                    mofanBtnOnclick("E", Axis.Y, isbool);
                    break;
                case 'M':
                    mofanBtnOnclick("M", Axis.X, isbool);
                    break;
                case 'S':
                    mofanBtnOnclick("S", Axis.Z, isbool);
                    break;
                case 'F':
                    mofanBtnOnclick("F", Axis.Z, isbool);
                    break;
                case 'B':
                    mofanBtnOnclick("B", Axis.Z, isbool);
                    break;
                case 'R':
                    mofanBtnOnclick("R", Axis.X, isbool);
                    break;
                case 'L':
                    mofanBtnOnclick("L", Axis.X, isbool);
                    break;
                case 'U':
                    mofanBtnOnclick("U", Axis.Y, isbool);
                    break;
                case 'D':
                    mofanBtnOnclick("D", Axis.Y, isbool);
                    break;
            }
        }
        //恢复默认转动为顺时针
        UIinit.btn_type = 1;
    }
    /// <summary>
    /// 魔方转动调用方法
    /// </summary>
    /// <param name="mofantype">控制器类型</param>
    /// <param name="axis">转动的轴</param>
    /// <param name="istip">是否转动完后刷新魔方公式提示</param>
    public void mofanBtnOnclick(string mofantype,Axis axis,bool istip)
    {
        //看传入的mofantype是否相同，相同则不重复生成控制器
        if (pre_mofantype != mofantype)
        {
            //决定魔方不同控制器的生成
            for (int i = 0; i < cubePrefabList.Length; i++)
            {
                mofanConCreate(cubePrefabList[i].transform.position, mofantype, axis);
                //先将魔方的方块全部移动到主体上
                cubePrefabList[i].transform.SetParent(mofan.transform);

                if (isadd)
                {
                    cubePrefabList[i].transform.SetParent(mofanControlList[mofanConNum].transform);
                }
            }
            //记录下魔方的类型
            pre_mofantype = mofantype;
        }

        //按钮类型决定魔方的旋转类型
        switch (UIinit.btn_type)
        {
            case 1:
                mofanRotation(mofantype, axis,90);
                break;
            case 2:
                mofanRotation(mofantype, axis, -90);
                break;
        }
        StartCoroutine(RotateOverTime(mofanControlList[mofanConNum].transform, Quaternion.Euler(mofan_rotate * n1), 0.23f,istip));
    }
    //规定时间内转动固定角度
    IEnumerator RotateOverTime(Transform transformToRotate, Quaternion targetRotation, float duration,bool istip)
    {
        isFinish = false;
        float timePassed = 0f;
        float factor;
        while (timePassed < duration)
        {
            factor = timePassed / duration;
            transformToRotate.rotation = Quaternion.Lerp(transformToRotate.rotation, targetRotation, factor);
            // 增加自上一帧起经过的时间
            yield return null;
            timePassed += Time.deltaTime;
        }
        // 要确保以精确值结束，请在完成时设置目标旋转修复
        transformToRotate.rotation = targetRotation;
        isFinish = true;
        //魔方转动后是否刷新魔方公式提示
        if (istip)
            MofanMixCube.Instance.CheckFan();
        yield return null;
    }
    //魔方旋转角度调用
    void mofanRotation(string mofantype,Axis axis, int rotype)
    {
        //决定魔方的旋转方位
        switch (axis)
        {
            case Axis.X:
                switch (mofantype)
                {
                    case "R":
                    case "r":
                        mofan_rotate = Vector3.right;
                        mofan_arr[0] += rotype;
                        n1 = mofan_arr[0];
                        break;
                    case "M":
                        mofan_rotate = Vector3.left;
                        mofan_arr[1] += rotype;
                        n1 = mofan_arr[1];
                        break;
                    case "L":
                    case "l":
                        mofan_rotate = Vector3.left;
                        mofan_arr[2] += rotype;
                        n1 = mofan_arr[2];
                        break;
                }
                break;
            case Axis.Y:
                switch (mofantype)
                {
                    case "U":
                    case "u":
                        mofan_rotate = Vector3.up;
                        mofan_arr[3] += rotype;
                        n1 = mofan_arr[3];
                        break;
                    case "E":
                        mofan_rotate = Vector3.down;
                        mofan_arr[4] += rotype;
                        n1 = mofan_arr[4];
                        break;
                    case "D":
                    case "d":
                        mofan_rotate = Vector3.down;
                        mofan_arr[5] += rotype;
                        n1 = mofan_arr[5];
                        break;
                }
                break;
            case Axis.Z:
                switch (mofantype)
                {
                    case "B":
                    case "b":
                        mofan_rotate = Vector3.forward;
                        mofan_arr[6] += rotype;
                        n1 = mofan_arr[6];
                        break;
                    case "S":
                        mofan_rotate = Vector3.back;
                        mofan_arr[7] += rotype;
                        n1 = mofan_arr[7];
                        break;
                    case "F":
                    case "f":
                        mofan_rotate = Vector3.back;
                        mofan_arr[8] += rotype;
                        n1 = mofan_arr[8];
                        break;
                }
                break;
            case Axis.O:
                switch (mofantype)
                {
                    case "x":
                        mofan_rotate = Vector3.right;
                        mofan_arr[9] += rotype;
                        n1 = mofan_arr[9];
                        break;
                    case "y":
                        mofan_rotate = Vector3.up;
                        mofan_arr[10] += rotype;
                        n1 = mofan_arr[10];
                        break;
                    case "z":
                        mofan_rotate = Vector3.back;
                        mofan_arr[11] += rotype;
                        n1 = mofan_arr[11];
                        break;
                }
                break;
        }
        for (int i = 0; i < mofan_arr.Length; i++)
        {
            if (mofan_arr[i] == 360| mofan_arr[i] == -360)
            {
                mofan_arr[i] = 0;
            }
        }
    }
    //魔方控制器生成
    void mofanConCreate(Vector3 mofanposition,string mofantype, Axis axis)
    {

        switch (axis)
        {
            case Axis.X:
                mofanposition.x = Mathf.Round(mofanposition.x);
                switch (mofantype)
                {
                    case "r":
                        isadd = ((mofanposition.x == 0) || (mofanposition.x == mofanControlList[2].transform.position.x));
                        mofanConNum = 2;
                        break;
                    case "l":
                        isadd = ((mofanposition.x == 0) || (mofanposition.x == mofanControlList[3].transform.position.x));
                        mofanConNum = 3;
                        break;
                    case "R":
                        isadd = (mofanposition.x == mofanControlList[2].transform.position.x);
                        mofanConNum = 2;
                        break;
                    case "L":
                        isadd = (mofanposition.x == mofanControlList[3].transform.position.x);
                        mofanConNum = 3;
                        break;
                    case "M":
                        isadd = (mofanposition.x == 0);
                        mofanConNum = 7;
                        break;
                }
                break;
            case Axis.Y:
                mofanposition.y = Mathf.Round(mofanposition.y);
                switch (mofantype)
                {
                    case "u":
                        isadd = ((mofanposition.y == 0) || (mofanposition.y == mofanControlList[4].transform.position.y));
                        mofanConNum = 4;
                        break;
                    case "d":
                        isadd = ((mofanposition.y == 0) || (mofanposition.y == mofanControlList[5].transform.position.y));
                        mofanConNum = 5;
                        break;
                    case "U":
                        isadd = (mofanposition.y == mofanControlList[4].transform.position.y);
                        mofanConNum = 4;
                        break;
                    case "D":
                        isadd = (mofanposition.y == mofanControlList[5].transform.position.y);
                        mofanConNum = 5;
                        break;
                    case "E":
                        isadd = (mofanposition.y == 0);
                        mofanConNum = 6;
                        break;
                }
                break;
            case Axis.Z:
                mofanposition.z= Mathf.Round(mofanposition.z);
                switch (mofantype)
                {
                    case "f":
                        isadd = ((mofanposition.z == 0) || (mofanposition.z == mofanControlList[0].transform.position.z));
                        mofanConNum = 0;
                        break;
                    case "b":
                        isadd = ((mofanposition.z == 0) || (mofanposition.z== mofanControlList[1].transform.position.z));
                        mofanConNum = 1;
                        break;
                    case "F":
                        isadd = (mofanposition.z== mofanControlList[0].transform.position.z);
                        mofanConNum = 0;
                        break;
                    case "B":
                        isadd = (mofanposition.z == mofanControlList[1].transform.position.z);
                        mofanConNum = 1;
                        break;
                    case "S":
                        isadd = (mofanposition.z == 0);
                        mofanConNum = 8;
                        break;
                }
                break;
            case Axis.O:
                isadd = true;
                switch (mofantype)
                {
                    case "x":
                        mofanConNum = 9;
                        break;
                    case "y":
                        mofanConNum = 10;
                        break;
                    case "z":
                        mofanConNum = 11;
                        break;
                }
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //退出游戏
            Application.Quit();
        }
    }
}
