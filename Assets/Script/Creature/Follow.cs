using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Vector3 distance = Vector3.zero;

    private void Start()
    {
        distance = transform.position - target.position;
    }

    private void Update()
    {
        transform.position = target.position + distance;
    }
}
