using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : StateMachine<EnemyController>
{
    private Animator anim;
    private NavMeshAgent nav;

    [SerializeField]
    protected int hp;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Attack attack;
    private Collider attackCollider;
    [SerializeField]
    private Transform target;//敵
    private float distance;
    [SerializeField]
    private float reactionDistance;//接敵に反応する距離
    [SerializeField]
    private float contactDistance;//攻撃できる距離
    [SerializeField]
    private float coolTime;//攻撃の感覚;
    private float leftCoolTime;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitSE;
    [SerializeField]
    private AudioClip attackSE;


    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        nav.speed = speed;
        anim = GetComponent<Animator>();
        attack.Initialize(gameObject);
        attackCollider = attack.gameObject.GetComponent<Collider>();

        ChangeState(new IdleState(this));
    }

    private void FixedUpdate()
    {
        OnUpdate();
    }

    protected void AttackOn()
    {
        audioSource.PlayOneShot(attackSE);
        attackCollider.enabled = true;
    }
    
    protected void AttackOff()
    {
        attackCollider.enabled = false;
    }

    private void OnExitAttack()
    {
        leftCoolTime = coolTime;
        ChangeState(new IdleState(this));
    }

    public void OnDamage(int damage)
    {
        audioSource.PlayOneShot(hitSE);
        hp -= damage;

        if (hp <= 0)
        {
            ChangeState(new DieState(this));
        }
        else
        {
            ChangeState(new DamegeState(this));
        }
    }

    private void OnExitDamage()
    {
        ChangeState(new IdleState(this));
    }


    private class IdleState : State<EnemyController>
    {
        public IdleState(EnemyController _m) : base(_m){}

        public override void OnEnter()
        {
            m.distance = Vector3.Distance(m.target.position, m.transform.position);
            m.anim.SetFloat("speed", 0, 0.1f, Time.fixedDeltaTime);
        }

        public override void OnUpdate()
        {
            m.distance = Vector3.Distance(m.target.position, m.transform.position);
            m.leftCoolTime -= Time.fixedDeltaTime;
            
            if (m.distance <= m.reactionDistance)
            {
                m.ChangeState(new EnemyController.RunState(m));
                return;
            }

            m.anim.SetFloat("speed", 0, 0.1f, Time.fixedDeltaTime);
        }
    }

    private class RunState : State<EnemyController>
    {
        private Vector2 move = Vector2.zero;
        private Quaternion targetRotation;

        public RunState(EnemyController _m) : base(_m){}

        public override void OnEnter()
        {
            m.distance = Vector3.Distance(m.target.position, m.transform.position);
            m.nav.destination = m.target.position;
            m.anim.SetFloat("speed", 1, 0.1f, Time.fixedDeltaTime);
        }

        public override void OnUpdate()
        {
            m.distance = Vector3.Distance(m.target.position, m.transform.position);
            m.leftCoolTime -= Time.fixedDeltaTime;

            if (m.distance > m.reactionDistance)
            {
                m.ChangeState(new EnemyController.IdleState(m));
                return;
            }
            else if (m.distance <= m.contactDistance)
            {
                m.ChangeState(new EnemyController.ContactState(m));
                return;
            }

            m.nav.destination = m.target.position;
            m.anim.SetFloat("speed", 1, 0.1f, Time.fixedDeltaTime);
        }
    }

    private class ContactState : State<EnemyController>
    {
        Quaternion rotation;

        public ContactState(EnemyController _m) : base(_m){}

        public override void OnEnter()
        {
            // 方向を、回転情報に変換
            rotation = Quaternion.LookRotation(m.target.position - m.transform.position);
            // 現在の回転情報と、ターゲット方向の回転情報を補完する
            m.transform.rotation  = Quaternion.Slerp(m.transform.rotation, rotation, 2 * Time.fixedDeltaTime);                   
            
            m.anim.SetFloat("speed", 0, 0.1f, Time.fixedDeltaTime);
        }

        public override void OnUpdate()
        {
            m.distance = Vector3.Distance(m.target.position, m.transform.position);
            m.leftCoolTime -= Time.fixedDeltaTime;
                
            if (m.distance > m.contactDistance)
            {
                m.ChangeState(new EnemyController.RunState(m));
                return;
            }
            else if (m.leftCoolTime <= 0)
            {
                m.ChangeState(new EnemyController.AttackState(m));
                return;
            }

            // 方向を、回転情報に変換
            rotation = Quaternion.LookRotation(m.target.position - m.transform.position);
            // 現在の回転情報と、ターゲット方向の回転情報を補完する
            m.transform.rotation  = Quaternion.Slerp(m.transform.rotation, rotation, 2 * Time.fixedDeltaTime);                   
            
            m.anim.SetFloat("speed", 0, 0.1f, Time.fixedDeltaTime);
        }
    }

    private class AttackState : State<EnemyController>
    {
        public AttackState(EnemyController _m) : base(_m){}

        public override void OnEnter()
        {
            m.anim.SetTrigger("isAttack");
        }

        public override void OnUpdate()
        {
            m.leftCoolTime -= Time.fixedDeltaTime;
        }

        public override void OnExit()
        {
            m.AttackOff();
        }
    }

    private class DamegeState : State<EnemyController>
    {
        public DamegeState(EnemyController _m) : base(_m){}

        public override void OnEnter()
        {
            m.anim.SetTrigger("isDamage");
        }

        public override void OnUpdate()
        {
            m.leftCoolTime -= Time.fixedDeltaTime;
        }
    }

    private class DieState : State<EnemyController>
    {
        public DieState(EnemyController _m) : base(_m){}

        public override void OnEnter()
        {
            m.nav.updatePosition = false;
            m.anim.SetTrigger("isDie");
            m.enabled = false;
            Destroy(m.gameObject, 5);
        }
    }
}
