using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T : MonoBehaviour
{
    public Vector3 tt;
    void Update()
    {
        Debug.DrawLine(this.transform.position, tt * 10, Color.blue);
        //Debug.DrawLine(this.transform.position, this.transform.right *10, Color.blue);
        //Debug.DrawLine(this.transform.position, this.transform.up * 10, Color.blue);
    }
}
