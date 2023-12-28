using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/MiniCamera")]
public class MiniCamera : MonoBehaviour
{
    public GameObject CubePrefab;
    GameObject obj;
    int count;
    //预览目标
    public Transform target;
    //观看魔方的距离，有缩放效果
    float distance = 6f;
    //物体观看速度
    public float xSpeed = 300f;
    public float ySpeed = 280f;
    //物体上下旋转角度的限制
    public float yMinLimit = -90.0f;
    public float yMaxLimit = 90.0f;
    //鼠标的移动
    private float x = 0.0f;
    private float y = 0.0f;
    //摄像机与魔方的距离
    Vector3 negDistance;
    //摄像机的角度
    Quaternion rotation;
    //摄像机的位置
    Vector3 position;

    void Start()
    {
        count = 0;
        //获取屏幕分辨率比例
        float ratio = (float)Screen.width / (float)Screen.height;
        //使摄像机视图永远是一个正方形，rect的前两个参数表示XY位置，后两个参数是XY大小
        this.GetComponent<Camera>().rect = new Rect(0, (1 - 0.2f * ratio), 0.2f, 0.2f * ratio);

        //获得摄像机在世界的欧拉角
        Vector3 angles = transform.eulerAngles;
        //摄像机与魔方的距离
        negDistance = new Vector3(0.0f, 0.0f, -distance);
        //摄像机的角度输入
        x = angles.x;
        y = angles.y;
        //初始化摄像机角度
        InFAngles();

        //生成27个魔方方块
        generateCubeSquare();
    }

    public void InFAngles()
    {
        this.x = -45f;
        this.y = 32.5f;
    }
    public void generateCubeSquare()
    {
        for (float x = -1; x < 2; x++)
        {
            for (float z = -1; z < 2; z ++)
            {
                for (float y = -1; y < 2; y ++)
                {
                    count++;
                    obj = Instantiate(CubePrefab, new Vector3(x, y, z), Quaternion.identity, this.transform);
                    obj.name = "V" + count;
                    obj.transform.SetParent(target);
                }
            }
        }
    }

    void Update()
    {
        if (target && Input.GetKey(KeyCode.V))
        {
            //锁定鼠标
            Cursor.lockState = CursorLockMode.Locked;
            //隐藏鼠标
            Cursor.visible = false;
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            x += dx * xSpeed * Time.deltaTime;
            y -= dy * ySpeed * Time.deltaTime;

            //限制 value的值在min,max之间,如果value大于max,则返回max,如果value小于min,则返回min,否者返回value
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            //释放鼠标
            Cursor.lockState = CursorLockMode.None;
            //显示鼠标
            Cursor.visible = true;
            InFAngles();
        }
        //更新摄像机
        UpdateRotaAndPos();
    }

    void UpdateRotaAndPos()
    {
        if (!target)
            return;
        rotation = Quaternion.Euler(y, x, 0);
        position = rotation * negDistance + target.position;
        //更新摄像机的位置和角度
        transform.rotation = rotation;
        transform.position = position;
    }



}
