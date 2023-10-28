using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UnitSpawner : MonoBehaviour
{
    public event Action<Unit> OnUnitSpawn;

    private IInputMode _inputMode;
    private UnitFactory _factory;
    private GameStatusController _gameStatus;

    private void OnEnable()
    {
        _inputMode.OnSpawnUnitButtonClick += SpawnSpiritUnit;
    }

    private void OnDisable()
    {
        _inputMode.OnSpawnUnitButtonClick -= SpawnSpiritUnit;
    }

    public void Init()
    {
        _factory.CreateUnits();
        SpawnHumanUnits();
    }

    public void SpawnUnit(Unit unit)
    {
        unit.gameObject.SetActive(true);
        unit.OnSpawn();

        OnUnitSpawn?.Invoke(unit);
    }

    private void SpawnHumanUnits()
    {
        foreach (var units in _factory.Units)
        {
            foreach (var unit in units.Value)
            {
                if (unit.Data.Team == UnitType.UnitTeam.Spirits) continue;
                SpawnUnit(unit);
            }
        }
    }

    private void SpawnSpiritUnit(Vector3 position)
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (_gameStatus.IsGameStarted || isPointerOverUI) return;

        Unit unit = _factory.FindFreeUnit(UnitFactory.UnitType.Skeleton);

        if (unit != null)
        {
            SpawnUnit(unit);
            unit.transform.position = position;
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode, UnitFactory factory, GameStatusController gameStatus)
    {
        _inputMode = inputMode;
        _factory = factory;
        _gameStatus = gameStatus;
    }
}
