using UnityEngine;

[CreateAssetMenu(fileName = "Unit data", menuName = "Units/Unit data")]
public class UnitDataConfig : ScriptableObject
{
    [SerializeField, Range(1, 200)] private int _health;
    [SerializeField, Range(1, 20)] private int _attackDamage;
    [SerializeField, Range(1, 10)] private float _speed = 2f;

    [SerializeField] private UnitType.UnitTeam _team;

    public int Health => _health;
    public int AttackDamage => _attackDamage;
    public float Speed => _speed;
    public UnitType.UnitTeam Team => _team;
}

public class UnitType
{
    public enum UnitTeam
    {
        None,
        Spirits,
        Humanity
    }
}
