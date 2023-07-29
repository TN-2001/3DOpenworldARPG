using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // 回転速度
    [SerializeField]
    private float speed = 1f;
    // 回転する軸
    [SerializeField]
    private bool x,y,z = false;

    // 前の時間
    private float beforeTime = 0f;

    private void Update()
    {
        // 回転
        Vector3 angle = transform.eulerAngles;
        if(x)
        {
            angle.x = angle.x  + 360 * speed * (Time.realtimeSinceStartup - beforeTime);
        }
        if(y)
        {
            angle.y = angle.y  + 360 * speed * (Time.realtimeSinceStartup - beforeTime);
        }
        if(z)
        {
            angle.z = angle.z  + 360 * speed * (Time.realtimeSinceStartup - beforeTime);
        }
        transform.eulerAngles = angle;

        // 時間保存
        beforeTime = Time.realtimeSinceStartup;
    }
}
