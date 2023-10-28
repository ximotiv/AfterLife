using System;
using Zenject;

public class UnitCounter
{
    public event Action OnUpdatedScore;

    private UnitSpawner _spawner;

    private int _spiritTeamScore;
    private int _humanityTeamScore;

    public int SpiritTeamScore => _spiritTeamScore;
    public int HumanityTeamScore => _humanityTeamScore;

    public void Init()
    {
        _spawner.OnUnitSpawn += AddCounter;
    }

    private void AddCounter(Unit unit)
    {
        if (unit.Data.Team == UnitType.UnitTeam.Spirits)
        {
            _spiritTeamScore++;
        }
        else
        {
            _humanityTeamScore++;
        }

        unit.GetSystem<HealthSystem>().OnDead += () =>
        {
            RemoveCounter(unit);
        };

        OnUpdatedScore?.Invoke();
    }

    private void RemoveCounter(Unit unit)
    {
        if (unit.Data.Team == UnitType.UnitTeam.Spirits)
        {
            _spiritTeamScore--;
        }
        else
        {
            _humanityTeamScore--;
        }

        unit.GetSystem<HealthSystem>().OnDead -= () =>
        {
            RemoveCounter(unit);
        };

        OnUpdatedScore?.Invoke();
    }

    [Inject]
    public void Construct(UnitSpawner spawner)
    {
        _spawner = spawner;
    }
}
