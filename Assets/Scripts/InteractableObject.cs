using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private InteractiveDataConfig _interactiveData;

    private Outline _outline;

    public bool IsSelected { get; protected set; }
    public bool IsAnimationActived { get; protected set; }
    public bool IsOnMouseEnter { get; private set; }

    protected virtual void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 0;

        ChangeOutlineColor(InteractiveDataConfig.OutlineType.MouseStay);
    }

    private void OnMouseEnter()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            ShowOutline();
            IsOnMouseEnter = true;
        }
    }

    private void OnMouseExit()
    {
        IsOnMouseEnter = false;

        if (IsSelected) return;

        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            HideOutline();
        }
    }

    private void OnMouseUp()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            //HandleClick();
        }
    }

    public void Select()
    {
        if (IsSelected) return;

        IsSelected = true;

        ShowOutline();
        ChangeOutlineColor(InteractiveDataConfig.OutlineType.Selected);

        //OnSelected();
    }

    public void UnSelect()
    {
        if (!IsSelected) return;

        IsSelected = false;

        ChangeOutlineColor(InteractiveDataConfig.OutlineType.MouseStay);

        if (!IsOnMouseEnter)
        {
            HideOutline();
        }

        //OnUnSelected();
    }

    public void ShowOutline()
    {
        _outline.OutlineWidth = 3;
    }

    public void HideOutline()
    {
        if (IsSelected) return;
        _outline.OutlineWidth = 0;
    }

    protected void ChangeOutlineColor(InteractiveDataConfig.OutlineType type)
    {
        if (_outline == null || _interactiveData == null) return;
        _outline.OutlineColor = _interactiveData.OutlineColors[(int)type];
    }

    //protected abstract void HandleClick();
}