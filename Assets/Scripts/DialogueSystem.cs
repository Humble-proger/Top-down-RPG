using System;
using UnityEngine;
public class DialogueSystem : MonoBehaviour 
{
    [Header("Reference")]
    [SerializeField] private PlayerActionMenuView _actionMenu;
    [SerializeField] private DialogUI _dialogUI;

    public static DialogueSystem Instance { get; private set; }
    public event Action<DialogueData, NPC> OnStartDialogue;
    public event Action OnEndDialogue;

    private DialogueData _currentDialogue;
    private DialogueNode _currentNode;
    private NPC _npc;

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

    public void StartDialog(DialogueData dialog, NPC npc)
    {
        _currentDialogue = dialog;
        _npc = npc;
        _currentNode = _currentDialogue.GetNodeByID(_currentDialogue.StartNodeID);

        OnStartDialogue?.Invoke(dialog, npc);
        _actionMenu.InitializeItems(ref _currentNode.Options);
        _actionMenu.gameObject.SetActive(true);
    }

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
        _actionMenu.Reset();
        _actionMenu.InitializeItems(ref _currentNode.Options);
    }

    //[TODO] Добавить функционал после того, как будет добавлены системы отношений и система квестов
    private void ProcessOptionConsequences(DialogueOption option) => throw new NotImplementedException();
    
    public void EndDialogue() 
    {
        _currentDialogue = null;
        _currentNode = null;
        _actionMenu.gameObject.SetActive(false);
        _actionMenu.Reset();

        OnEndDialogue?.Invoke();
    }
}