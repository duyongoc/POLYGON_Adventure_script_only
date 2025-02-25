using UnityEngine;

public class ActorRanger : ActorBase
{


    [Header("[Config]")]
    [SerializeField] private SOActorRanger CONFIG;
    [SerializeField] private ActorRangerSFX actorSFX;
    [SerializeField] private ActorRangerVFX actorVFX;
    [SerializeField] private ActorRangerAttack actorAttack;

    [Header("[State Current]")]
    [SerializeField] private ActorState currentState;
    [SerializeField] private ActorState previousState;

    [Header("[State Component]")]
    [SerializeField] private RangerStateHit stateHit;
    [SerializeField] private RangerStateIdle stateIdle;
    [SerializeField] private RangerStateMove stateMove;
    [SerializeField] private RangerStateDead stateDead;
    [SerializeField] private RangerStateAttack stateAttack;


    // [private]
    private float _attackRange;
    private float _attackDetection;
    private BattleController _mapController;


    // [properties]
    public ActorRangerAttack Attack { get => actorAttack; }
    public ActorRangerSFX SFX { get => actorSFX; set => actorSFX = value; }
    public ActorRangerVFX VFX { get => actorVFX; set => actorVFX = value; }



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

        SetStats();
        SetBaseStats();
        SetBaseUnityValue();

        // SetInitActor();
        // SetInitTarget();
        // SetNavAgentEnable();
        SetStateIdle();

        stateIdle.Init(this);
        stateMove.Init(this);
        stateAttack.Init(this);

        actorAttack.Init(CONFIG, this);
        ActorHealth.Init(CONFIG.defaultStats.hp);
    }


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




    // public override void SetPreviousState()
    // {
    //     SetState(previousState);
    // }


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


    // // public override void ForceAnimationKnockUp()
    // // {
    // //     isAttacking = true;
    // //     animator.SetBool("Run", false);
    // //     animator.SetBool("Idle", false);
    // //     animator.SetTrigger(ANIM_EFFECT_KNOCK_UP_AND_LYING);
    // // }


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


    // public override void UpdateHit(SOCCBase effect, bool playAimHit = false)
    // {
    //     SetState(stateHit);
    //     stateHit.UpdateEffectSide(effect, playAimHit);
    //     actorAttack.AddCombatPoint();
    //     highlightEffect.HitFXWhite();
    // }


    // public override void TakeDamage(EntityBase source, SOCCBase effect, float damage, bool playAimHit)
    // {
    //     DecreaseHP(damage, playAimHit);
    //     UpdateHit(effect, playAimHit);
    // }


    public override void OnDead()
    {
        base.OnDead();
        SetState(stateDead);
        // actorSFX.Play_MonsterDead();
        // actorVFX.Play_ParticleDeath();

        // MissionController.Instance.RemoveEnemy(this);
        // Utils.Delay(timeDestroyActor, SelfDestroy);
    }


}
