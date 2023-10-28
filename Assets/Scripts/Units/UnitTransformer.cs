using DG.Tweening;
using UnityEngine;
using Zenject;

public class UnitTransformer
{
    private UnitFactory _factory;
    private UnitSpawner _spawner;
    private FightController _fightController;

    public void Init()
    {
        _factory.OnUnitCreated += OnUnitCreated;
    }

    private void OnUnitCreated(Unit unit)
    {
        if (unit.Data.Team == UnitType.UnitTeam.Spirits) return;

        unit.GetSystem<HealthSystem>().OnDead += () => 
        {
            TransformationToSkeleton(unit);
        };
    }

    private void TransformationToSkeleton(Unit unit)
    {
        unit.transform.DOMoveY(-1, 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Unit skeleton = _factory.CreateSkeleton(unit.transform.position, Quaternion.identity);
            skeleton.transform.DOMoveY(0.3f, 3f).SetEase(Ease.Linear);
            _spawner.SpawnUnit(skeleton);

            FindTarget(skeleton);

            Object.Destroy(unit.gameObject);
        });
    }

    private void FindTarget(Unit unit)
    {
        Unit target = _fightController.FindNearestEnemy(unit);
        if (target != null)
        {
            unit.GetSystem<AISystem>().Attack(target);
        }
    }

    [Inject]
    public void Construct(UnitFactory factory, UnitSpawner spawner, FightController fightController)
    {
        _factory = factory;
        _spawner = spawner;
        _fightController = fightController;
    }
}
