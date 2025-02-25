using UnityEngine;

public class ActorHero : ActorBase
{


    [Header("[Hero]")]
    [SerializeField] private SOActorHero CONFIG;
    [SerializeField] private ActorHeroSFX actorSFX;
    [SerializeField] private ActorHeroVFX actorVFX;
    [SerializeField] private ActorHeroAttack actorAttack;

    [Header("[State Component]")]
    [SerializeField] private HeroStateHit stateHit;
    [SerializeField] private HeroStateIdle stateIdle;
    [SerializeField] private HeroStateMove stateMove;
    [SerializeField] private HeroStateDead stateDead;
    [SerializeField] private HeroStateAttack stateAttack;

    [Header("[State Current]")]
    [SerializeField] private ActorState currentState;
    [SerializeField] private ActorState previousState;


    // [private]
    private float _attackRange;
    private float _attackDetection;
    private BattleController _mapController;


    // [properties]
    public ActorHeroVFX VFX { get => actorVFX; }
    public ActorHeroSFX SFX { get => actorSFX; }
    public ActorHeroAttack Attack { get => actorAttack; }



    #region UNITY
    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (IsDead || _mapController.IsGameFinished)
            return;

        OnUpdateEnemy();
        currentState.UpdateState();
    }
    #endregion




    public void Init()
    {
        _mapController = BattleController.Instance;

        SetInit();
        SetStats();
        SetBaseStats();
        SetBaseUnityValue();
        SetStateIdle();

        stateIdle.Init(this);
        stateMove.Init(this);
        stateAttack.Init(this);
        actorAttack.Init(CONFIG, this);
        ActorHealth.Init(CONFIG.defaultStats.hp);
    }


    // so bad code - will improve later
    private void SetStats()
    {
        component.DefaultStats = CONFIG.defaultStats;
        component.PriorityFocus = CONFIG.priorityFocus;

        _attackRange = CONFIG.attackRange;
        _attackDetection = CONFIG.attackDetection;
    }


    public void SetState(ActorState newState)
    {
        if (currentState != null)
            currentState.EndState();

        previousState = currentState;
        currentState = newState;
        currentState.StartState();
    }



    public override void SetStateHit()
    {
        SetState(stateHit);
    }

    public override void SetStateIdle()
    {
        SetState(stateIdle);
    }

    public override void SetStateMove()
    {
        SetState(stateMove);
    }

    public override void SetStateDead()
    {
        SetState(stateDead);
    }

    public override void SetStateAttack()
    {
        SetState(stateAttack);
    }

    public override void SetPreviousState()
    {
        SetState(previousState);
    }



    public override void SetMove(Vector3 position, bool forceMove)
    {
        if (!forceMove && currentState.Equals(stateAttack))
            return;

        ResetAttack();
        SetStateMove();
        SetNavMeshMoveToPosition(position);
        stateMove.SetMovePosition(position);
    }



    public bool IsInAttackRange()
    {
        if (CurrentEnemy == null)
            return false;

        var distance = Vector3.Distance(CurrentEnemy.transform.position, transform.position);
        if (distance <= _attackRange)
            return true;

        return false;
    }


    public bool IsInAttackDetection()
    {
        if (CurrentEnemy == null)
            return false;

        var distance = Vector3.Distance(CurrentEnemy.transform.position, transform.position);
        if (distance <= _attackDetection)
            return true;

        return false;
    }



    public override void UpdateHit(SOCC cc, bool playAimHit = false)
    {
        SetState(stateHit);
        stateHit.UpdateHit(cc, playAimHit);
    }


    public override void OnDead()
    {
        base.OnDead();
        SetState(stateDead);
        // actorSFX.Play_MonsterDead();
        // actorVFX.Play_ParticleDeath();
        // Utils.Delay(timeDestroyActor, SelfDestroy);
    }




    // public void SetStateHit(SOCCBase effect)
    // {
    //     SetState(stateHit);
    //     stateHit.UpdateEffectSide(effect);
    // }


    // public override void ResetAttackCountdown()
    // {
    //     // print("timeAttackCountdown");
    //     progressAttackCountdown = 0;
    // }


    // public override void ForceAnimationKnockUp()
    // {
    //     isAttacking = true;
    //     animator.SetBool("Run", false);
    //     animator.SetBool("Idle", false);
    //     animator.SetTrigger(ANIM_EFFECT_KNOCK_UP_AND_LYING);
    // }


    // public override void PlayAnimationHit()
    // {
    //     // skip when character is moving
    //     if (previousState == stateMoving)
    //         return;

    //     actorSFX.Play_MonsterHit();
    //     actorVFX.Play_ParticleHit();
    //     PlayAnimationHitByDirection();
    //     OnFinishedAttack();
    //     ResetAttackCountdown();
    // }


    // public override void TakeDamage(EntityBase source, SOCCBase effect, float damage, bool playAimHit)
    // {
    //     DecreaseHP(damage, playAimHit);
    //     UpdateHit(effect, playAimHit);
    // }


}
