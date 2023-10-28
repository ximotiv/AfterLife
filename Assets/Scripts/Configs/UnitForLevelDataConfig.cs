using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Unit Level Data")]
public class UnitForLevelDataConfig : ScriptableObject
{
    [SerializeField] private UnitFactory.UnitType _unitType;
    [SerializeField] private Unit _unitPrefab;

    [SerializeField] private Vector3[] _unitStartPositions;

    public UnitFactory.UnitType UnitType => _unitType;
    public Unit UnitPrefab => _unitPrefab;
    public IReadOnlyList<Vector3> StartPositions => _unitStartPositions;
}
