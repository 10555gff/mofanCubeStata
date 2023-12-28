using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MofanCreate : MonoBehaviour
{
    public GameObject CubePrefab;
    GameObject obj;
    int count=0;
    string[] cube_control = { "F_Control", "B_Control", "R_Control", "L_Control", "U_Control", "D_Control" ,
                              "E_Control", "M_Control", "S_Control", "x_Control", "y_Control", "z_Control" };
    Vector3[] cube_position = { new Vector3(0, 0, -1), new Vector3(0, 0, 1), 
                                new Vector3(1, 0, 0),new Vector3(-1, 0, 0),
                                new Vector3(0, 1, 0),new Vector3(0, -1, 0),
                                new Vector3(0,0,0)};
    List<GameObject> CubePrefabList = new List<GameObject>();
    void Start()
    {
        //生成6个面控制器,6个中心块控制器
        cubeControlCreate();
        //生成27个魔方方块
        generateCubeSquare();
    }
    public void generateCubeSquare()
    {
        for(float x = -1; x < 2; x++)
        {
            for(float z = -1; z < 2; z ++)
            {
                for (float y = -1; y < 2; y ++)
                {
                    count++;
                    obj=Instantiate(CubePrefab, new Vector3(x, y, z), Quaternion.identity, this.transform);
                    obj.name = "M"+count;

                }
            }
        }
    }
    public void cubeControlCreate()
    {
        for (int i = 0; i < cube_control.Length; i++)
        {
            CubePrefabList.Add(new GameObject(cube_control[i]));
            if (i < 6)
            {
                CubePrefabList[i].transform.position = cube_position[i];
                
            }
            else
            {
                CubePrefabList[i].transform.position = cube_position[cube_position.Length-1];
            }
            CubePrefabList[i].transform.SetParent(this.transform);
            CubePrefabList[i].tag = "mofan_control";
        }
    }
}
