public abstract class InputState 
{
    protected InputController _inputContainer;
    public InputState(InputController inputContainer) => _inputContainer = inputContainer;
    public abstract TypeInput TypeState { get; }
    public abstract void Enter();
    public abstract void Exit();
}