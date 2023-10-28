using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnitFactory;

public class UnitFactory : MonoBehaviour
{
    public event Action<Unit> OnUnitCreated;

    [SerializeField] private Unit _skeleton;
    [SerializeField] private LevelDataConfig _levelData;
    
    [SerializeField] private Transform _container;
    [Inject] private readonly DiContainer _diContainer;

    private readonly Dictionary<UnitType, List<Unit>> _units = new();
    public IReadOnlyDictionary<UnitType, List<Unit>> Units => _units;

    public enum UnitType
    {
        Skeleton,
        Man
    }

    public void CreateUnits()
    {
        for (int i = 0; i < _levelData.Units.Count; i++)
        {
            for (int u = 0; u < _levelData.Units[i].StartPositions.Count; u++)
            {
                CreateUnit(_levelData.Units[i], _levelData.Units[i].StartPositions[u]);
            }
        }
    }

    public Unit FindFreeUnit(UnitType unitType)
    {
        Unit freeUnit = null;

        foreach (var unit in _units[unitType])
        {
            if (unit.gameObject.activeSelf) continue;
            freeUnit = unit;
            break;
        }

        return freeUnit;
    }

    public Unit CreateSkeleton(Vector3 position, Quaternion rotation)
    {
        Unit unit = _diContainer.InstantiatePrefab(_skeleton, position, Quaternion.identity, _container).GetComponent<Unit>();
        unit.Init();

        AddToList(unit, UnitType.Skeleton);

        OnUnitCreated?.Invoke(unit);
        return unit;
    }

    private Unit CreateUnit(UnitForLevelDataConfig unitConfig, Vector3 position)
    {
        Unit unit = _diContainer.InstantiatePrefab(unitConfig.UnitPrefab, position, Quaternion.identity, _container).GetComponent<Unit>();
        unit.Init();
        unit.gameObject.SetActive(false);

        AddToList(unit, unitConfig.UnitType);

        OnUnitCreated?.Invoke(unit);
        return unit;
    }

    private void AddToList(Unit unit, UnitType type)
    {
        if (!_units.ContainsKey(type))
        {
            _units[type] = new List<Unit>();
        }
        _units[type].Add(unit);
    }
}
