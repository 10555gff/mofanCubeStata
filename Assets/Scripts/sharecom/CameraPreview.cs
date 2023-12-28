using UnityEngine;
using System.Collections;

public class CameraPreview : MonoBehaviour
{
    //预览目标
    public Transform target;
    //观看魔方的距离，有缩放效果
    public float distance = 8.0f;
    //物体观看速度
    public float xSpeed = 250f;
    public float ySpeed = 200f;
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

    // 用于初始化
    void Start()
    {
        //获得摄像机在世界的欧拉角
        Vector3 angles = transform.eulerAngles;
        //摄像机与魔方的距离
        negDistance = new Vector3(0.0f, 0.0f, -distance);
        //摄像机的角度输入
        x = angles.x;
        y = angles.y;
        //初始化摄像机角度
        InFAngles();
    }
    public void InFAngles()
    {
        this.x = -45f;
        this.y = 30f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //	发射射线（从摄像机发射一条射线经过屏幕上的点击位置）
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {

                Debug.Log(hitInfo.collider.name + ":" + MofanRestore.Instance.MofanStata(hitInfo.collider.name));
                //  击中物体
                //Debug.Log(hitInfo.collider.name + ":" + hitInfo.collider.transform.position + ":" + hitInfo.collider.transform.eulerAngles + ":" + hitInfo.collider.transform.rotation);
                //Debug.Log(MofanRestore.Instance.MofanStata(hitInfo.collider.name)+":"+ hitInfo.collider.name);
            }
        }
        if (target && Input.GetKey(KeyCode.LeftAlt))
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
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            //释放鼠标
            Cursor.lockState = CursorLockMode.None;
            //显示鼠标
            Cursor.visible = true;
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