using Godot;
using System.Collections.Generic;

public interface Switch
{
    public bool CurrentState { get; set; }
    public List<IInteractable> Interactables { get; set; }

    public void Init(params object[] interactables)
    {
        Interactables = new List<IInteractable>();
        foreach (var item in interactables)
        {
            if (item is IInteractable interactable)
                Interactables.Add(interactable);
        }
    }

    protected void OnStateChanged(bool newState);

    public bool SetState(bool newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnStateChanged(newState);
        }

        foreach (IInteractable item in Interactables)
        {
            item.SetState(CurrentState);
        }

        return CurrentState;
    }
}