using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : CollisionDetector
{
    private void Start()
    {
        onTriggerEnter.AddListener(OnAttack);
    }

    private void OnAttack(Collider other)
    {
        other.GetComponent<IBattler>().Damage(1);
    }
}
