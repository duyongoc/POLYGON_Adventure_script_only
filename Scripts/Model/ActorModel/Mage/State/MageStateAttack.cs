using UnityEngine;

public class MageStateAttack : ActorStateAttack
{


    // [private]
    private ActorMage _owner;


    public void Init(ActorMage actor)
    {
        _owner = actor;

    }


    public override void StartState()
    {
    }

    public override void EndState()
    {
    }


    public override void UpdateState()
    {
        _owner.UpdateClosestEnemy();
        if (_owner.IsInAttackRange())
        {
            if (_owner.IsAttacking)
                return;
                
            _owner.SetNavMeshStop(true);
            _owner.LookAtZeroCurrentEnemy();
            Attack();
        }
        else
        {
            _owner.SetStateIdle();
        }
    }


    private void Attack()
    {
        _owner.AttackDelayTimer += Time.deltaTime;
        if (_owner.AttackDelayTimer >= _owner.AttackDelay)
        {
            _owner.Attack.DoAttack(_owner.CurrentEnemy);
            _owner.AttackDelayTimer = 0;
        }
    }


}
