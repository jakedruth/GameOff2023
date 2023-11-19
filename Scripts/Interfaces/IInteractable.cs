using Godot;
public interface IInteractable
{
    public bool CurrentState { get; set; }
    protected void OnStateChanged(bool newState);

    public bool SetState(bool newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnStateChanged(CurrentState);
        }

        return CurrentState;
    }
}