using UnityEngine;
using Zenject;

public class FightController
{
    private UnitFactory _factory;
    private GameStatusController _gameStatus;

    public void Init()
    {
        _gameStatus.OnGameStarted += StartFight;
    }

    private void StartFight()
    {
        foreach (var unitList in _factory.Units.Values)
        {
            foreach (var unit in unitList)
            {
                if (!unit.gameObject.activeSelf) continue;

                SetTarget(unit);

                AISystem ai = unit.GetSystem<AISystem>();
                ai.OnTargetDead += SetTarget;
            }
        }
    }

    private void SetTarget(Unit unit)
    { 
        AISystem ai = unit.GetSystem<AISystem>();
        if (unit.IsDead)
        {
            ai.OnTargetDead -= SetTarget;
            ai.Stop();
        }
        else
        {
            Unit target = FindNearestEnemy(unit);

            if (target != null)
            {
                ai.Attack(target);
            }
        }
    }

    public Unit FindNearestEnemy(Unit currentUnit)
    {
        Unit nearestEnemy = null;
        float minDistance = float.PositiveInfinity;

        foreach (var unitList in _factory.Units.Values)
        {
            foreach (var enemyUnit in unitList)
            {
                bool isActive = !enemyUnit.IsDead && enemyUnit.gameObject.activeSelf;
                bool isAnotherTeam = enemyUnit.Data.Team != currentUnit.Data.Team;

                if (isActive && enemyUnit != currentUnit && isAnotherTeam)
                {
                    float distance = Vector3.Distance(currentUnit.transform.position, enemyUnit.transform.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = enemyUnit;
                    }
                }
            }
        }

        return nearestEnemy;
    }

    [Inject]
    public void Construct(UnitFactory factory, GameStatusController gameStatus)
    {
        _factory = factory;
        _gameStatus = gameStatus;
    }
}
