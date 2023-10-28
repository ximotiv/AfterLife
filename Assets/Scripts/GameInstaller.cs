using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private UnitFactory _unitFactory;
    [SerializeField] private UnitSpawner _unitSpawner;

    [SerializeField] private GameStatusController _gameStatus;
    
    private UnitCounter _unitCounter;

    private readonly IInputMode _inputMode = new MouseKeyboardInput();
    private FightController _fightController;
    private UnitTransformer _unitTransformer;

    private void Awake()
    {
        Resolves();

        _inputMode.Init();

        _unitTransformer.Init();
        _unitCounter.Init();
        _unitSpawner.Init();

        _fightController.Init();
    }

    public override void InstallBindings()
    {
        Container.Bind<IInputMode>().FromInstance(_inputMode).AsSingle();

        InitUnits();

        Container.Bind<GameStatusController>().FromInstance(_gameStatus).AsSingle();

        Container.Bind<AnimationCache>().FromNew().AsSingle();

        Container.Bind<FightController>().FromNew().AsSingle();
    }

    private void InitUnits()
    {
        Container.Bind<UnitSpawner>().FromInstance(_unitSpawner).AsSingle();
        Container.Bind<UnitFactory>().FromInstance(_unitFactory).AsSingle();

        Container.Bind<UnitCounter>().FromNew().AsSingle();
        Container.Bind<UnitTransformer>().FromNew().AsSingle();
    }

    private void Resolves()
    {
        _fightController = Container.Resolve<FightController>();
        _unitTransformer = Container.Resolve<UnitTransformer>();
        _unitCounter = Container.Resolve<UnitCounter>();
    }
}
