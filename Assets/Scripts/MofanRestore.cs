using System;
using UnityEngine;

public class MofanRestore : MonoBehaviour
{
    public static MofanRestore Instance = null;
    //当前读取的魔方块
    Transform curCubetransform;
    int curMoIndex;

    string str;
    string mofanColor;
    private void Start()
    {
        MofanRestore.Instance = this;
    }
    public string MofanStata(string mofanName)
    {
        int n= int.Parse(mofanName.Substring(1)) - 1;
        curCubetransform = GameManager.Instance.cubePrefabList[n].transform;
        mofanColor = mofanReColor(mofanName);
        //区别棱块与角块
        switch (mofanColor.Length)
        {
            case 2:
                curMoIndex = mofancubeStat();
                return string.Format("{0}{1}{2}{3}", str[0], mofanColor[curMoIndex], str[1], mofanColor[curMoIndex ^ 1]);
            case 3:
                string tmpStr = swapStr();
                return string.Format("{0}{1}{2}{3}{4}{5}", str[0], mofanColor[curMoIndex], str[1], tmpStr[0], str[2], tmpStr[1]);
        }
        return null;
    }

    //点乘判断,用得1或-1，返回0
    int reAbs(Vector3 v1, Vector3 v2)
    {
        return (int)Mathf.Abs(Vector3.Dot(v1, v2)) ^ 1;
    }
    #region 棱块判断块
    //用点乘判断是棱块状态
    int mofancubeStat()
    {
        Vector3 v3 = reInitVector();
        Vector3 v3fu = isCenter() ? this.transform.forward : this.transform.up;
        return reAbs(v3, v3fu);
    }

    //中间棱块与其它棱块分别定义初始轴
    Vector3 reInitVector()
    {
        switch (curCubetransform.name)
        {
            //中间层定义初始轴方向
            case "M20":
            case "M2":
            case "M26":
            case "M8":
                return curCubetransform.forward;
            //其它层定义初始轴方向
            default:
                return curCubetransform.up;
        }
    }

    //棱块的位置，是位置！！！ 是否在中间层
    bool isCenter()
    {
        str = pType(curCubetransform.position.ToString());
        if (str[0] != 'F' && str[0] != 'B') return false;
        return true;
    }
    #endregion

    #region 角块判断块
    //用点乘判断是角块状态
    int mofancubeSamStat()
    {
        Vector3 v1 = curCubetransform.up;
        Vector3 v2 = curCubetransform.forward;
        Vector3 v3 = curCubetransform.right;
        Vector3 vtup = this.transform.up;
        //依次对比
        if (reAbs(v1, vtup) == 0)
        {
            //未旋转
            return 0;
        }else if (reAbs(v2, vtup) == 0)
        {
            //顺时钟旋转
            return 1;
        }
        else if (reAbs(v3, vtup) == 0)
        {
            //逆时钟旋转
            return 2;
        }
        return -1;
    }

    string swapStr()
    {
        str = pType(curCubetransform.position.ToString());
        curMoIndex = mofancubeSamStat();
        char a = mofanColor[(curMoIndex + 1) % 3];
        char b = mofanColor[(curMoIndex + 2) % 3];
        //判断某个角块所处位置，同一个角块，不同位置有着两种不同状态
        switch (str)
        {
            case "UFR":
            case "DFL":
            case "DBR":
            case "UBL":
                return isEn() ? $"{a}{b}" : $"{b}{a}";
            case "DFR":
            case "UFL":
            case "UBR":
            case "DBL":
                return isEn() ? $"{b}{a}" : $"{a}{b}";
        }
        return "  ";
    }

    //判断相反,旁边全是敌人，对面才是友军
    bool isEn()
    {
        switch (curCubetransform.name) {
            case "M21":
            case "M9":
            case "M1":
            case "M25":
                return true;
            case "M3":
            case "M27":
            case "M19":
            case "M7":
                return false;
        }
        return false;
    }
    #endregion

    //总20个位置，位置转成可读字符编号
    public string pType(string s)
    {
        switch (s)
        {
            #region 棱块
            //顶棱
            case "(0.0, 1.0, -1.0)":
                return "UF";
            case "(1.0, 1.0, 0.0)":
                return "UR";
            case "(0.0, 1.0, 1.0)":
                return "UB";
            case "(-1.0, 1.0, 0.0)":
                return "UL";
            //底棱
            case "(0.0, -1.0, -1.0)":
                return "DF";
            case "(1.0, -1.0, 0.0)":
                return "DR";
            case "(0.0, -1.0, 1.0)":
                return "DB";
            case "(-1.0, -1.0, 0.0)":
                return "DL";
            //前侧棱
            case "(1.0, 0.0, -1.0)":
                return "FR";
            case "(-1.0, 0.0, -1.0)":
                return "FL";
            //后侧棱
            case "(1.0, 0.0, 1.0)":
                return "BR";
            case "(-1.0, 0.0, 1.0)":
                return "BL";
            #endregion
            #region 角块
            //顶角块
            case "(1.0, 1.0, -1.0)":
                return "UFR";
            case "(-1.0, 1.0, -1.0)":
                return "UFL";
            case "(1.0, 1.0, 1.0)":
                return "UBR";
            case "(-1.0, 1.0, 1.0)":
                return "UBL";

            //底角块
            case "(1.0, -1.0, -1.0)":
                return "DFR";
            case "(-1.0, -1.0, -1.0)":
                return "DFL";
            case "(1.0, -1.0, 1.0)":
                return "DBR";
            case "(-1.0, -1.0, 1.0)":
                return "DBL";
            #endregion
            default:
                return "";
        }
    }

    //魔方块编号转对应颜色
    public string mofanReColor(string mofanName)
    {
        switch (mofanName)
        {
            #region 中心块
            case "M11":
                return "绿";
            case "M15":
                return "黄";
            case "M5":
                return "红";
            case "M23":
                return "橙";
            case "M17":
                return "蓝";
            case "M13":
                return "白";
            #endregion
            #region 角块
            case "M21":
                return "黄绿橙";
            case "M3":
                return "黄绿红";
            case "M27":
                return "黄蓝橙";
            case "M9":
                return "黄蓝红";
            case "M19":
                return "白绿橙";
            case "M1":
                return "白绿红";
            case "M25":
                return "白蓝橙";
            case "M7":
                return "白蓝红";
            #endregion
            #region 棱块
            case "M20":
                return "绿橙";
            case "M2":
                return "绿红";
            case "M26":
                return "蓝橙";
            case "M8":
                return "蓝红";

            case "M12":
                return "黄绿";
            case "M24":
                return "黄橙";
            case "M6":
                return "黄红";
            case "M18":
                return "黄蓝";

            case "M10":
                return "白绿";
            case "M22":
                return "白橙";
            case "M4":
                return "白红";
            case "M16":
                return "白蓝";
                #endregion
        }
        return "";
    }

    internal string readMofanOrder(string v)
    {
        throw new NotImplementedException();
    }

    internal string readMofanFixed(string v, bool isHaveColour)
    {
        throw new NotImplementedException();
    }
}
