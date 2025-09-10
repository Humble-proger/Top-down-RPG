using UnityEngine;

public class TransferItemSystem : MonoBehaviour {

    [SerializeField] private TransferItemView _view;
    public static TransferItemSystem Instance { get; private set; }

    private ContainerInformation _sourceInfo;
    private ContainerInformation _targetInfo;

    private void OnEnable()
    {
        _view.Ñonfirm += OnConfirmTransfer;
        _view.Cancel += OnCancelTransfer;
    }

    private void OnDisable()
    {
        _view.Ñonfirm -= OnConfirmTransfer;
        _view.Cancel -= OnCancelTransfer;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnCancelTransfer()
    {
        _sourceInfo.Clear();
        _targetInfo.Clear();
    }

    private void OnConfirmTransfer(int obj)
    {
        if (!_sourceInfo.IsEmpty() && !_targetInfo.IsEmpty()) {
            if (!InventoryManager.Instance.TrySendBetweenInventories(
                    _sourceInfo.NumSlot, _targetInfo.NumSlot,
                    _sourceInfo.ContainerId, _targetInfo.ContainerId, obj))
            {
                LoggerService.Debug("(TransferItemSystem) Couldn't move the element.");
            }
        }
        OnCancelTransfer();
    }

    public void TransferItem(IDragContainer source, IDragContainer destination) 
    {
        if (source.IsEmpty())
        {
            LoggerService.Debug("(TransferItemSystem) Attempt to move an empty element.");
            return;
        }
        
        ContainerInformation sourceInformation = source.GetItem();
        ContainerInformation destinationInformation = destination.GetItem();

        if (destination.IsEmpty() || source.Equal(destination))
        {
            if (sourceInformation.Quantity > 1 && InventoryManager.Instance.CanAddToInventory(sourceInformation.ItemType, destinationInformation.ContainerId))
            {
                _sourceInfo = sourceInformation;
                _targetInfo = destinationInformation;
                _view.SetMaxSlider(sourceInformation.Quantity);
                _view.ResetSlider();
                _view.Enable();
            }
            else if (sourceInformation.Quantity == 1)
            {
                if (!InventoryManager.Instance.TrySendBetweenInventories(
                    sourceInformation.NumSlot, destinationInformation.NumSlot,
                    sourceInformation.ContainerId, destinationInformation.ContainerId))
                {
                    LoggerService.Debug("(TransferItemSystem) Couldn't move the element.");
                }
            }
            else
            {
                LoggerService.Warning("(TransferItemSystem) The number of the element is a negative number.");
            }
        }
        else if (!InventoryManager.Instance.TrySwapBetweenInventories(sourceInformation.NumSlot, destinationInformation.NumSlot,
            sourceInformation.ContainerId, destinationInformation.ContainerId)) {
            LoggerService.Debug("(TransferItemSystem) Couldn't exchange items.");
        }
    }
}