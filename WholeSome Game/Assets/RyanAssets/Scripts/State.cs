
public abstract class State
{
    internal virtual StateMachine machine { get; set; }
    public bool isActive
    {
        get
        {
            return machine.IsCurrentState(GetType());
        }
    }

    public virtual void StateSetup() { }
    public virtual void StateEnter() { }
    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateLateUpdate() { }
    public virtual void OnAnimatorIK(int layerIndex) { }
    public virtual void StateExit() { }
}
