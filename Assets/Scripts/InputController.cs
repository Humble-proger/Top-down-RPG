using UnityEngine;

public class InputController : MonoBehaviour 
{
    private InputSystem _inputHandler;
    
    public static InputController Instance { get; private set; }

    public Vector2 MousePosition { get; private set; }
    public Vector2 Move { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _inputHandler = new InputSystem();
    }

    private void OnEnable()
    {
        _inputHandler.Player.Enable();
        _inputHandler.Player.Mouse.performed += OnMousePositionChanged;
        _inputHandler.Player.Mouse.canceled += OnMousePositionChanged;
        _inputHandler.Player.Move.performed += OnMoveChanged;
        _inputHandler.Player.Move.canceled += OnMoveChanged;
    }

    private void OnDisable()
    {
        _inputHandler.Player.Mouse.performed -= OnMousePositionChanged;
        _inputHandler.Player.Mouse.canceled -= OnMousePositionChanged;
        _inputHandler.Player.Move.performed -= OnMoveChanged;
        _inputHandler.Player.Move.canceled -= OnMoveChanged;
        _inputHandler.Player.Disable();
    }

    private void OnMoveChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Move = _inputHandler.Player.Move.ReadValue<Vector2>();
    }

    private void OnMousePositionChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        MousePosition = _inputHandler.Player.Mouse.ReadValue<Vector2>();
    }
}
