using UnityEngine;

public class InputController : MonoBehaviour 
{
    private InputState _currentState;
    
    public static InputController Instance { get; private set; }

    public InputSystem InputHandler { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InputHandler = new();

        ChangeState(TypeInput.Player);
    }

    public void ChangeState(TypeInput type)
    {
        InputState state = InitiateState(type);

        _currentState?.Exit();
        _currentState = state;
        _currentState?.Enter();
    }

    private InputState InitiateState(TypeInput type)
    {
        switch (type)
        {
            case TypeInput.Player:
                return new InputPlayerState(Instance);
            case TypeInput.Dialog:
                return new InputDealogState(Instance);
        }

        LoggerService.Critical("(InputController) An unknown type has been introduced.");
        return null;
    }
}