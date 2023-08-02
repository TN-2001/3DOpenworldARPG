using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    // 当たり判定の対象のタグ
    [SerializeField]
    private string tagName = null;
    // 当たり判定対象のタグ持ちの中でも無視するオブジェクト
    [SerializeField]
    private GameObject ignoreObject = null;

    // イベントを使わない場合
    // フラグ
    public bool isCollision { get; private set; } = false;
    // 衝突中のオブジェクト
    public GameObject collisionObject { get; private set; } = null;

    // イベントのみを使う場合
    // 引数にColliderを持ったUnityEvent
    public UnityEvent<Collider> onTriggerEnter = null;
    public UnityEvent<Collider> onTriggerStay = null;
    public UnityEvent<Collider> onTriggerExit = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == tagName && other.gameObject != ignoreObject)
        {
            isCollision = true;
            collisionObject = other.gameObject;
            onTriggerEnter?.Invoke(other);        
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == tagName && other.gameObject != ignoreObject)
        {
            onTriggerStay?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == tagName && other.gameObject != ignoreObject)
        {
            isCollision = false;
            collisionObject = null;
            onTriggerExit?.Invoke(other);
        }
    }
}
