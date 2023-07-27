using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform loadImgTra;
    [SerializeField]
    private float rotateSpeed;
    private float timeCount;
    private float z;

    private void Update()
    {
        timeCount += Time.deltaTime;
        z = timeCount * rotateSpeed;

        if (z >= 360)
        {
            z = z - 360;
        }

        loadImgTra.eulerAngles = new Vector3(0, 0, -z);
    }
}
