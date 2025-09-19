using UnityEngine;
public class DialogueSystem : MonoBehaviour 
{
    [Header("Reference")]
    [SerializeField] private PlayerActionMenuView _actionMenu;

    public static DialogueSystem Instance { get; private set; }

    private DialogueData _currentDialogue;
    private DialogueNode _currentNode;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool InDialogue => _currentDialogue != null;

    ///[TODO]
    public void StartDialog(DialogueData dialog, IInteractible npc)
    {
    
    }

    //[TODO]
    public void SelectOption(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= _currentNode.Options.Count) {
            LoggerService.Warning("(DialogueSystem) optionIdex out of range.");
            return;
        }

        DialogueOption option = _currentNode.Options[optionIndex];
        if (!option.IsAvailable) {
            LoggerService.Warning("(DialogueSystem) An unavailable option is selected.");
            return;
        }
        // Обрабатываем последствия выбора данной опции
        ProcessOptionConsequences(option);

        // Обрабатываем переход к следующему узлу
        if (string.IsNullOrEmpty(option.NextNodeID)) {
            EndDialogue();
            LoggerService.Debug("(DialogueSystem) There is no next node. The dialog is closed...");
            return;
        }

        _currentNode = _currentDialogue.GetNodeByID(option.NextNodeID);
        if (_currentNode == null) {
            LoggerService.Error($"(DialogueSystem) The specified Node '{option.NextNodeID}' does not exist");
            EndDialogue();
            return;
        }

    }

    //[TODO]
    private void ProcessOptionConsequences(DialogueOption option) { }
    
    //[TODO]
    public void EndDialogue() { }
}