using System;
using UnityEngine.InputSystem;

[Serializable]
public class InputDealogState : InputState
{
    public override TypeInput TypeState => TypeInput.Dialog;
    public static event Action DialogPrevious;
    public static event Action DialogNext;
    public static event Action DialogSelect;
    public static event Action DialogCancel;
    public static float ScrollValue { get; private set; }

    public InputDealogState(InputController inputContainer) : base(inputContainer) {}
  
    public override void Enter()
    {
        _inputContainer.InputHandler.Dialog.Enable();
        _inputContainer.InputHandler.Dialog.Next.performed += OnDialogNext;
        _inputContainer.InputHandler.Dialog.Prev.performed += OnDialogPrev;
        _inputContainer.InputHandler.Dialog.Select.performed += OnDialogSelected;
        _inputContainer.InputHandler.Dialog.Scroll.performed += OnScrollDialog;
        _inputContainer.InputHandler.Dialog.Scroll.canceled += OnScrollDialog;
        _inputContainer.InputHandler.Dialog.Exit.performed += OnExitDialog;
    }

    public override void Exit()
    {
        _inputContainer.InputHandler.Dialog.Next.performed -= OnDialogNext;
        _inputContainer.InputHandler.Dialog.Prev.performed -= OnDialogPrev;
        _inputContainer.InputHandler.Dialog.Select.performed -= OnDialogSelected;
        _inputContainer.InputHandler.Dialog.Scroll.performed -= OnScrollDialog;
        _inputContainer.InputHandler.Dialog.Scroll.canceled -= OnScrollDialog;
        _inputContainer.InputHandler.Dialog.Exit.performed -= OnExitDialog;
        _inputContainer.InputHandler.Dialog.Disable();
        ScrollValue = 0f;
    }

    private void OnExitDialog(InputAction.CallbackContext _)
    {
        DialogCancel?.Invoke();
    }

    private void OnDialogPrev(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        DialogPrevious?.Invoke();
    }

    private void OnDialogNext(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        DialogNext?.Invoke();
    }

    private void OnDialogSelected(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        DialogSelect?.Invoke();
    }
    private void OnScrollDialog(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        ScrollValue = _inputContainer.InputHandler.Dialog.Scroll.ReadValue<float>();
    }
}