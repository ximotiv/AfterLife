using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISystem : IUnitSystem
{
    public event Action<Unit> OnTargetDead;

    private readonly StateMachine _stateMachine = new();
    public IState CurrentState => _stateMachine.CurrentState;

    private readonly Unit _unit;
    private readonly NavMeshAgent _agent;

    private Coroutine _timer;

    public AISystem(Unit unit)
    {
        _unit = unit;
        _agent = unit.GetComponent<NavMeshAgent>();
    }

    public void Init()
    {
        InitStates();
    }

    public void Waiting()
    {
        IState waitingState = _stateMachine.GetState<WaitingState>();
        _stateMachine.ChangeState(waitingState);

        StartTimer();
    }

    public void Attack(Unit target)
    {
        IState aggressive = _stateMachine.GetState<AggressiveState>();
        _stateMachine.ChangeState(aggressive);

        AggressiveState state = (AggressiveState)aggressive;
        state.SetTarget(target);

        StartTimer();
    }

    public void Stop()
    {
        if (_timer != null)
        {
            _unit.StopCoroutine(_timer);
        }

        _stateMachine.CurrentState.Exit();
    }

    public void StopAttack()
    {
        OnTargetDead?.Invoke(_unit);
    }

    private void InitStates()
    {
        _stateMachine.StateMap = new Dictionary<Type, IState>
        {
            [typeof(WaitingState)]  = new WaitingState(_unit),
            [typeof(AggressiveState)]  = new AggressiveState(_unit, _agent),
        };
    }

    private void StartTimer()
    {
        if (_timer != null)
        {
            _unit.StopCoroutine(_timer);
        }

        _timer = _unit.StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        WaitForSeconds waitForSeconds = new(0.5f);
        yield return waitForSeconds;

        while (_stateMachine.CurrentState != null)
        {
            _stateMachine.CurrentState.Update();
            yield return waitForSeconds;
        }
    }
}
