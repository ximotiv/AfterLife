using UnityEngine;

public class AnimationCache
{
    private readonly int _idleIndex;
    private readonly int _walkIndex;
    private readonly int _runIndex;
    private readonly int _winIndex;
    private readonly int _deadIndex;
    private readonly int _attackIndex;

    public int IdleIndex => _idleIndex;
    public int WalkIndex => _walkIndex;
    public int RunIndex => _runIndex;
    public int WinIndex => _winIndex;
    public int DeadIndex => _deadIndex;
    public int AttackIndex => _attackIndex;

    public AnimationCache()
    {
        _idleIndex = Animator.StringToHash(StringBus.ANIM_IDLE);
        _walkIndex = Animator.StringToHash(StringBus.ANIM_WALK);
        _runIndex = Animator.StringToHash(StringBus.ANIM_RUN);
        _winIndex = Animator.StringToHash(StringBus.ANIM_WIN);
        _deadIndex = Animator.StringToHash(StringBus.ANIM_DEAD);
        _attackIndex = Animator.StringToHash(StringBus.ANIM_ATTACK);
    }
}

