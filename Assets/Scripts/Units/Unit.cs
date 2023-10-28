using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public abstract class Unit : InteractableObject
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnitDataConfig _config;

    private readonly List<IUnitSystem> _systems = new();

    private AnimationCache _animCache;

    private bool _isDead;

    public UnitDataConfig Data { get => _config; }
    public Animator Animator { get => _animator; }
    public AnimationCache AnimCache => _animCache;
    public bool IsDead => _isDead;

    public virtual void Init()
    {
        HealthSystem health = new(_config.Health);
        health.OnDead += OnDead;

        AddSystem(health);

        AISystem ai = new(this);
        ai.Init();

        AddSystem(ai);
    }

    public void AddSystem(IUnitSystem system)
    {
        _systems.Add(system);
    }

    public void RemoveSystem(IUnitSystem system)
    {
        _systems.Remove(system);
    }

    public T GetSystem<T>() where T : IUnitSystem
    {
        return _systems.OfType<T>().FirstOrDefault();
    }

    public virtual void OnSpawn()
    {
        _isDead = false;

        AISystem ai = GetSystem<AISystem>();
        ai?.Waiting();

        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, 0.5f).SetEase(Ease.OutBack);
    }

    protected virtual void OnDead()
    {
        _isDead = true;

        GetSystem<AISystem>().Stop();
        _animator.SetTrigger(_animCache.DeadIndex);
    }

    [Inject]
    public void Construct(AnimationCache animCache)
    {
        _animCache = animCache;
    }
}
