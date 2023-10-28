using System;
using UnityEngine;

public interface IInputMode
{
    public event Action<Vector3> OnSpawnUnitButtonClick;
    public event Action OnRemoveUnitButtonClick;

    public void CheckInput();
    public void Init();
}
