using UnityEngine;
using UnityEngine.AI;

public class AggressiveState : UnitState
{
    private readonly NavMeshAgent _agent;
    private readonly float _attackDistance = 1.2f;

    private Unit _target;
    private HealthSystem _targetHealth;
    private float _attackDelay;

    public AggressiveState(Unit unit, NavMeshAgent agent) : base(unit)
    {
        _agent = agent;
    }

    public override void Enter()
    {
        _agent.speed = Unit.Data.Speed;
    }

    public override void Update()
    {
        if (_target == null || _target.IsDead || !_agent.isOnNavMesh || !_agent.isActiveAndEnabled) return;

        float distance = Vector3.Distance(_target.transform.position, Unit.transform.position);
        if (distance <= _attackDistance)
        {
            Unit.Animator.SetBool(Unit.AnimCache.RunIndex, false);
            Attack();
        }
        else
        {
            MoveToTarget();
        }
    }

    public override void Exit()
    {
        if (_targetHealth != null)
        {
            _targetHealth.OnDead -= OnTargetDead;
        }

        Stop();
    }

    public void SetTarget(Unit target)
    {
        _target = target;
        _targetHealth = target.GetSystem<HealthSystem>();
        _targetHealth.OnDead += OnTargetDead;
    }

    private void OnTargetDead()
    {
        Stop();

        if (_targetHealth != null)
        {
            _targetHealth.OnDead -= OnTargetDead;
        }

        Unit.GetSystem<AISystem>().StopAttack();
    }

    private void Attack()
    {
        if (_attackDelay > Time.time) return;

        HealthSystem targetHealth = _target.GetSystem<HealthSystem>();
        if (targetHealth != null)
        {
            Stop();
            Animate();

            targetHealth.TakeDamage(Unit.Data.AttackDamage);

            float delay = 2f;
            _attackDelay = Time.time + delay;
        }
    }

    private void MoveToTarget()
    {
        if (_agent.isOnNavMesh && _agent.isActiveAndEnabled)
        {
            _agent.isStopped = false;
        }

        _agent.SetDestination(_target.transform.position);
        Unit.Animator.SetBool(Unit.AnimCache.RunIndex, true);
    }

    private void Animate()
    {
        Unit.transform.LookAt(_target.transform);

        int randomAttackAnim = Random.Range(1, 5);
        Unit.Animator.SetInteger(Unit.AnimCache.AttackIndex, randomAttackAnim);
    }

    private void Stop()
    {
        if (_agent.isOnNavMesh && _agent.isActiveAndEnabled)
        {
            _agent.isStopped = true;
        }

        Unit.Animator.SetInteger(Unit.AnimCache.AttackIndex, 0);
        Unit.Animator.SetBool(Unit.AnimCache.RunIndex, false);
    }
}
