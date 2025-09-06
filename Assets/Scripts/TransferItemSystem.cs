using System;
using System.Linq;
using UnityEngine;

public class TransferItemSystem : MonoBehaviour {

    [SerializeField] private TransferItemView _view;
    public static TransferItemSystem Instance { get; private set; }

    private InventoryEntry<ItemData> _val;
    private IDragSource<ItemData> _source;
    private IDragDestination<ItemData> _destination;
    private int? _slotSource = null;

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

    private void OnCancelTransfer()
    {
        _source = null;
        _destination = null;
    }

    private void OnConfirmTransfer(int obj)
    {
        if (_source != null && _destination != null) {
            ProcessingAddTransfer(_source, _destination, _val, obj, _slotSource);
            _source = null;
            _destination = null;
            _slotSource = null;
        }
    }

    private void ProcessingAddTransfer(IDragSource<ItemData> source, IDragDestination<ItemData> destination, InventoryEntry<ItemData> val, int count, int? slotSource = null)
    {
        int passedObjectsCount;
        if (slotSource == null)
            passedObjectsCount = destination.AddItem(val.Count == count ? val : new InventoryEntry<ItemData>() { Item = val.Item, Count = count });
        else
            passedObjectsCount = destination.AddItem(val.Count == count ? val : new InventoryEntry<ItemData>() { Item = val.Item, Count = count }, (int) slotSource);
        if (passedObjectsCount < 0 || passedObjectsCount > count)
        {
            Debug.LogWarning("TransferWarning !!! An error occurred while adding objects.");
            return;
        }
        if (passedObjectsCount == 0)
            return;
        if (passedObjectsCount == val.Count)
        {
            source.RemoveItem();
        }
        else if (passedObjectsCount < val.Count)
        {
            int removeObjectCount = source.RemoveItem(passedObjectsCount);
            if (removeObjectCount != passedObjectsCount)
                Debug.LogWarning($"TransferWarning !!! Remove {removeObjectCount} object(s), but {passedObjectsCount} were expected.");
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TransferItem(IDragSource<ItemData> source, IDragDestination<ItemData> destination) 
    {
        ContainerInformation sourceInformation = source.GetSenderInformation();
        ContainerInformation destinationInformation = destination.GetDestinationInformation();

        if (!destinationInformation.Permitted.Contains(sourceInformation.ContainerType) ||
            !sourceInformation.Permitted.Contains(destinationInformation.ContainerType)) {
            return;
        }

        var item = source.GetItem();
        
        if (destination.Equal(item.Item))
        {
            if (item.Count > 0)
            {
                if (item.Count == 1)
                {
                    if (sourceInformation.ContainerId == destinationInformation.ContainerId)
                        ProcessingAddTransfer(source, destination, item, 1, sourceInformation.NumSlot);
                    else
                        ProcessingAddTransfer(source, destination, item, 1);
                }
                else
                {
                    _val = item;
                    _source = source;
                    _destination = destination;
                    _view.SetMaxSlider(item.Count);
                    _view.ResetSlider();
                    _view.Enable();
                }
            }
        }
        else if (destination.IsEmpty()) {
            if (sourceInformation.ContainerId == destinationInformation.ContainerId)
                ProcessingAddTransfer(source, destination, item, item.Count, sourceInformation.NumSlot);
            else 
                ProcessingAddTransfer(source, destination, item, item.Count);
        }
    }
}