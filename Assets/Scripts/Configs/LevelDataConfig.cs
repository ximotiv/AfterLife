using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelDataConfig : ScriptableObject
{
    [SerializeField] private int _skeletonCount;

    [SerializeField] private UnitForLevelDataConfig[] _units;

    public int SkeletonCount => _skeletonCount;
    public IReadOnlyList<UnitForLevelDataConfig> Units => _units;
}