using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : StateMachine<PlayerController>
{
    // コンポーネント
    private Animator anim;
    private CharacterController con;
    private PlayerInput input;

    [SerializeField]
    private GameUI gameUI;
    [SerializeField]
    private int hp;
    [SerializeField]
    private float stamina;
    private float maxStamina;
    private bool isStamina = true;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float rollSpeed;
    private bool isInvincible = false;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitSE;
    [SerializeField]
    private AudioClip attackSE;
    [SerializeField]
    private Attack attack;
    private Collider attackCollider;


    private void Start()
    {
        anim = GetComponent<Animator>();
        con = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();

        attack.Initialize(gameObject);
        attackCollider = attack.gameObject.GetComponent<Collider>();

        gameUI.InitialzeHpSlider(hp);
        maxStamina = stamina;
        gameUI.InitializeStaminaSlider(maxStamina);

        ChangeState(new IdleState(this));
    }

    private void OnExitRoll()
    {
        ChangeState(new IdleState(this));
    }

    private void AttackOn()
    {
        attackCollider.enabled = true;
        audioSource.PlayOneShot(attackSE);
    }
    
    private void AttackOff()
    {
        attackCollider.enabled = false;
    }

    private void OnExitAttack()
    {
        ChangeState(new IdleState(this));
    }

    public void OnDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        audioSource.PlayOneShot(hitSE);
        hp -= damage;
        gameUI.SetHpSlider(hp);

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


    private class IdleState : State<PlayerController>
    {
        public IdleState(PlayerController _m) : base(_m){}

        public override void OnEnter()
        {
            m.anim.SetFloat("speed", 0, 0.1f, Time.fixedDeltaTime);
        }

        public override void OnUpdate()
        {
            if (m.stamina < m.maxStamina)
            {
                m.stamina += Time.fixedDeltaTime / 5;
                
                if (m.stamina > 0.3f && !m.isStamina)
                {
                    m.isStamina = true;
                }
            }
            else
            {
                m.stamina = m.maxStamina;
            }
            m.gameUI.SetStaminaSlider(m.stamina);

            if (m.input.actions["Attack"].WasPressedThisFrame())
            {
                m.ChangeState(new PlayerController.AttackState(m));
                return;
            }
            else if (m.input.actions["Roll"].WasPressedThisFrame())
            {
                m.ChangeState(new PlayerController.RollState(m));
                return;
            }
            else if (m.input.actions["Move"].ReadValue<Vector2>().magnitude != 0)
            {
                m.ChangeState(new PlayerController.MoveState(m));
                return;
            }

            m.anim.SetFloat("speed", 0, 0.1f, Time.fixedDeltaTime);
        }
    }

    private class MoveState : State<PlayerController>
    {
        private Vector2 move = Vector2.zero;
        private Quaternion targetRotation;

        public MoveState(PlayerController _m) : base(_m){}

        public override void OnEnter()
        {
            move = m.input.actions["Move"].ReadValue<Vector2>();

            Quaternion horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            Vector3 dir = horizontalRotation * new Vector3(move.x, 0, move.y).normalized;

            // 向き
            targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            m.transform.rotation = Quaternion.RotateTowards(m.transform.rotation, targetRotation, 600 * Time.fixedDeltaTime);
            
            // 移動
            dir = new Vector3(dir.x, - m.gravity * Time.fixedDeltaTime, dir.z);

            if (m.input.actions["Dash"].IsPressed() && m.stamina > 0)
            {
                m.con.Move(dir * m.runSpeed * Time.fixedDeltaTime);
                m.anim.SetFloat("speed", 1, 0.1f, Time.fixedDeltaTime);
            }
            else
            {
                m.con.Move(dir * m.walkSpeed * Time.fixedDeltaTime);
                m.anim.SetFloat("speed", 0.5f, 0.1f, Time.fixedDeltaTime);
            }
        }

        public override void OnUpdate()
        {
            move = m.input.actions["Move"].ReadValue<Vector2>();

            if (m.input.actions["Attack"].WasPressedThisFrame())
            {
                m.ChangeState(new PlayerController.AttackState(m));
                return;
            }
            else if (m.input.actions["Roll"].WasPressedThisFrame())
            {
                m.ChangeState(new PlayerController.RollState(m));
                return;
            }
            else if (move.magnitude == 0)
            {
                m.ChangeState(new PlayerController.IdleState(m));
                return;
            }

            Quaternion horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            Vector3 dir = horizontalRotation * new Vector3(move.x, 0, move.y).normalized;

            // 向き
            targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            m.transform.rotation = Quaternion.RotateTowards(m.transform.rotation, targetRotation, 600 * Time.fixedDeltaTime);
            
            // 移動
            dir = new Vector3(dir.x, - m.gravity * Time.fixedDeltaTime, dir.z);

            if (m.input.actions["Dash"].IsPressed() && m.stamina > 0 && m.isStamina)
            {
                m.con.Move(dir * m.runSpeed * Time.fixedDeltaTime);
                m.anim.SetFloat("speed", 1, 0.1f, Time.fixedDeltaTime);

                m.stamina -= Time.fixedDeltaTime / 10;
                if (m.stamina <= 0)
                {
                    m.isStamina = false;
                }
                m.gameUI.SetStaminaSlider(m.stamina);
            }
            else
            {
                m.con.Move(dir * m.walkSpeed * Time.fixedDeltaTime);
                m.anim.SetFloat("speed", 0.5f, 0.1f, Time.fixedDeltaTime);

                if (m.stamina < m.maxStamina)
                {
                    m.stamina += Time.fixedDeltaTime / 5;

                    if (m.stamina > 0.3f && !m.isStamina)
                    {
                        m.isStamina = true;
                    }
                }
                else
                {
                    m.stamina = m.maxStamina;
                }
                m.gameUI.SetStaminaSlider(m.stamina);
            }
        }
    }

    private class RollState : State<PlayerController>
    {
        public RollState(PlayerController _m) : base(_m){}

        public override void OnEnter()
        {
            m.isInvincible = true;
            m.anim.SetTrigger("isRoll");

            Vector3 dir = new Vector3(m.transform.forward.x, - m.gravity, m.transform.forward.z);

            m.con.Move(dir * m.rollSpeed * Time.fixedDeltaTime);
        }

        public override void OnUpdate()
        {
            Vector3 dir = new Vector3(m.transform.forward.x, - m.gravity, m.transform.forward.z);

            m.con.Move(dir * m.rollSpeed * Time.fixedDeltaTime);
        }

        public override void OnExit()
        {
            m.isInvincible = false;
        }
    }

    private class AttackState : State<PlayerController>
    {
        public AttackState(PlayerController _m) : base(_m){}

        public override void OnEnter()
        {
            m.anim.SetTrigger("isAttack");
        }

        public override void OnUpdate()
        {
            if (m.input.actions["Attack"].WasPressedThisFrame())
            {
                m.ChangeState(new PlayerController.AttackState(m));
                return;
            }
        }

        public override void OnExit()
        {
            m.AttackOff();
        }
    }

    private class DamegeState : State<PlayerController>
    {
        public DamegeState(PlayerController _m) : base(_m){}

        public override void OnEnter()
        {
            m.anim.SetTrigger("isDamage");
        }
    }

    private class DieState : State<PlayerController>
    {
        public DieState(PlayerController _m) : base(_m){}

        public override void OnEnter()
        {
            m.anim.SetTrigger("isDie");
            m.enabled = false;
            MainManager.I.DiePlayer();
        }
    }
}