using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // 無視するオブジェクト（攻撃するオブジェクト）
    private GameObject obj;

    public void Initialize(GameObject _obj)
    {
        obj = _obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creature" && other.gameObject != obj)
        {
            if (other.GetComponent<PlayerController>())
            {
                other.GetComponent<PlayerController>().OnDamage(1);
            }
            else if (other.GetComponent<EnemyController>())
            {
                other.GetComponent<EnemyController>().OnDamage(1);
            }
        }
    }
}
