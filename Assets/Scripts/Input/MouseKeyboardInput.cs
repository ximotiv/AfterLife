using System;
using UnityEngine;

public class MouseKeyboardInput : IInputMode
{
    private Camera _camera;
    private LayerMask _groundMask;

    public event Action<Vector3> OnSpawnUnitButtonClick;
    public event Action OnRemoveUnitButtonClick;

    public void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
            {
                Vector3 targetPosition = hit.point + Vector3.up;
                OnSpawnUnitButtonClick?.Invoke(targetPosition);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRemoveUnitButtonClick?.Invoke();
        }
    }

    public void Init()
    {
        _camera = Camera.main;
        _groundMask = LayerMask.GetMask(StringBus.GroundLayer);
    }
}
