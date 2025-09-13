using System;

[Serializable]
public class InputPlayerState : InputState
{
    public InputPlayerState(InputController inputContainer) : base(inputContainer) {}

    public override TypeInput TypeState => TypeInput.Player;
    public static UnityEngine.Vector2 MousePosition { get; private set; }
    public static UnityEngine.Vector2 Move { get; private set; }


    public override void Enter()
    {
        _inputContainer.InputHandler.Player.Enable();
        _inputContainer.InputHandler.Player.Mouse.performed += OnMousePositionChanged;
        _inputContainer.InputHandler.Player.Mouse.canceled += OnMousePositionChanged;
        _inputContainer.InputHandler.Player.Move.performed += OnMoveChanged;
        _inputContainer.InputHandler.Player.Move.canceled += OnMoveChanged;
    }

    public override void Exit()
    {
        _inputContainer.InputHandler.Player.Mouse.performed -= OnMousePositionChanged;
        _inputContainer.InputHandler.Player.Mouse.canceled -= OnMousePositionChanged;
        _inputContainer.InputHandler.Player.Move.performed -= OnMoveChanged;
        _inputContainer.InputHandler.Player.Move.canceled -= OnMoveChanged;
        _inputContainer.InputHandler.Player.Disable();
        Move = UnityEngine.Vector2.zero;
    }
    private void OnMoveChanged(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        Move = _inputContainer.InputHandler.Player.Move.ReadValue<UnityEngine.Vector2>();
    }

    public void OnMousePositionChanged(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        MousePosition = _inputContainer.InputHandler.Player.Mouse.ReadValue<UnityEngine.Vector2>();
    }
}