using UnityEngine;

public class ActorMageAttack : MonoBehaviour
{


    [Header("[Setting]")]
    [SerializeField] private MageAttackNormal normal;
    [SerializeField] private MageAttackSkill_1 skill_1;
    [SerializeField] private MageAttackSkill_2 skill_2;
    [SerializeField] private MageAttackSkill_3 skill_3;


    // [private]
    private ActorMage _owner;


    // [properties]
    public MageAttackNormal AttackNormal { get => normal; }
    public MageAttackSkill_1 AttackSkill_1 { get => skill_1; }
    public MageAttackSkill_2 AttackSkill_2 { get => skill_2; }
    public MageAttackSkill_3 AttackSkill_3 { get => skill_3; }




    #region UNITY
    // private void Start()
    // {
    // }

    private void FixedUpdate()
    {
        if (_owner == null || _owner.IsDead)
            return;

        var vel = _owner.Agent.velocity.magnitude;
        _owner.Animator.SetFloat("MoveBlend", vel);
    }
    #endregion




    public void Init(SOActorMage SO, ActorMage actor)
    {
        _owner = actor;

        normal.Init(SO.attackNormal, actor);
        skill_1.Init(SO.attackSkill_1, actor);
        skill_2.Init(SO.attackSkill_2, actor);
        skill_3.Init(SO.attackSkill_3, actor);
    }


    public void DoAttack(UnitBase enemy)
    {
        // check condition attack skill 1
        if (CheckPlay_AttackSkill_1())
            return;

        // check condition attack skill 2
        if (CheckPlay_AttackSkill_2())
            return;

        // check condition attack skill 3
        if (CheckPlay_AttackSkill_3())
            return;

        OnTrigger_AttackNormal();
    }



    public bool CheckPlay_AttackSkill_1()
    {
        if (skill_1.CanPlayAttack())
        {
            OnTrigger_AttackSkill_1();
            return true;
        }
        return false;
    }

    public bool CheckPlay_AttackSkill_2()
    {
        if (skill_2.CanPlayAttack())
        {
            OnTrigger_AttackSkill_2();
            return true;
        }
        return false;
    }

    public bool CheckPlay_AttackSkill_3()
    {
        if (skill_3.CanPlayAttack())
        {
            OnTrigger_AttackSkill_3();
            return true;
        }
        return false;
    }




    private void OnTrigger_AttackNormal()
    {
        OnStartSkill();
        ResetAttackCountdown();

        _owner.Animator.SetTrigger(ActorBase.ANIM_ATTACK_NORMAL);
    }

    private void OnTrigger_AttackSkill_1()
    {
        OnStartSkill();
        ResetAttackCountdown();

        _owner.Animator.SetTrigger(ActorBase.ANIM_ATTACK_SKILL_1);
    }

    private void OnTrigger_AttackSkill_2()
    {
        OnStartSkill();
        ResetAttackCountdown();

        _owner.Animator.SetTrigger(ActorBase.ANIM_ATTACK_SKILL_2);
    }

    private void OnTrigger_AttackSkill_3()
    {
        OnStartSkill();
        ResetAttackCountdown();

        _owner.Animator.SetTrigger(ActorBase.ANIM_ATTACK_SKILL_3);
    }



    public void OnStartSkill()
    {
        _owner.OnStartAttack();
    }

    public void OnFinishAttack()
    {
        _owner.OnFinishedAttack();
    }

    public void ResetAttackCountdown()
    {
        _owner.ResetAttackDelay();
    }



}
