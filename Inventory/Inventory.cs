using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

///<Summary>
/// Inventory class supporting a hotbar. Uses input actions that will need to be defined.
///</Summary>

namespace Noba.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] int maxInventorySlots = 4;
        [SerializeField] GameObject itemAttachmentPoint;

        public Item CurrentHotbarObject { get; private set; } = null;
        public bool HandFull { get { return CurrentHotbarObject != null; } }

        List<Item> inventory = new List<Item>();

        float currentHotbarSlot = 0;

        InventoryInputActions inputActions;
        InputAction hotkey;
        InputAction drop;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            inputActions = new InventoryInputActions();

            hotkey = inputActions.Player.Hotkey;
            drop = inputActions.Player.Drop;

            hotkey.Enable();
            drop.Enable();

            for (int i = 0; i < maxInventorySlots; i++)
            {
                inventory.Add(null);
            }
        }

        void Update()
        {
            if (hotkey.IsPressed())
            {
                SelectHotbarItem();
            }

            if (drop.IsPressed())
            {
                if (CurrentHotbarObject)
                {
                    RemoveItem(CurrentHotbarObject);
                }
            }
        }

        void CheckSpentItems()
        {
            for (int i = inventory.Count - 1; i >= 0; i--)
            {
                Item item = inventory[i];

                if (item.Spent)
                {
                    RemoveItem(item, false);
                }
            }
        }

        public void RegisterHotbarSlot(int hotbarSlot, Item item)
        {
            inventory[hotbarSlot] = item;
        }

        int FindFreeHotbarSlot()
        {
            for (int i = 0; i < maxInventorySlots; i++)
            {
                if (!inventory[i])
                {
                    return i;
                }
            }

            return -1;
        }

        void SelectHotbarItem()
        {
            float lastHotbarSlot = currentHotbarSlot;
            currentHotbarSlot = hotkey.ReadValue<float>() - 1;

            UpdateSelectedHotbarItem();
        }

        void UpdateSelectedHotbarItem()
        {
            if (CurrentHotbarObject)
            {
                Item itemScript = CurrentHotbarObject.GetComponent<Item>();
                itemScript.SetActive(false);
            }

            CurrentHotbarObject = inventory[(int)currentHotbarSlot];

            if (CurrentHotbarObject)
            {
                Item itemScript = CurrentHotbarObject.GetComponent<Item>();
                itemScript.SetActive(true);
            }
        }

        int CheckFilledSlots()
        {
            int filledSlots = 0;

            for (int i = 0; i < maxInventorySlots; i++)
            {
                if (inventory[i])
                {
                    filledSlots += 1;
                }
            }

            return filledSlots;
        }

        int FindHotbarIndex(Item item)
        {
            if (!inventory.Contains(item))
            {
                return -1;
            }

            for (int i = 0; i < maxInventorySlots; i++)
            {
                if (inventory[i] == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public void PickUpItem(GameObject itemObject)
        {
            if (!itemObject)
            {
                Debug.Log("Object is null.");
                return;
            }

            if (CheckFilledSlots() >= maxInventorySlots)
            {
                Debug.Log("Inventory is full.");
                return;
            }

            Item itemScript = itemObject.GetComponent<Item>();

            if (!itemScript)
            {
                Debug.Log("Not an item.");
                return;
            }

            if (itemScript.Pickup(itemAttachmentPoint, gameObject))
            {
                RegisterItem(itemScript);
                itemScript.SetActive(false);
            }
        }

        public void RemoveItem(Item item, bool drop = true)
        {
            if (!inventory.Contains(item))
            {
                Debug.Log("Not in inventory.");
                return;
            }

            if (!drop)
            {
                item.Destroy();

                UnregisterItem(item);

                return;
            }

            if (item.Drop(gameObject))
            {
                UnregisterItem(item);

                if (item == CurrentHotbarObject)
                {
                    CurrentHotbarObject = null;
                }
            }
        }

        public bool TryTakeItem(Item item)
        {
            if (!inventory.Contains(item))
            {
                Debug.Log("Not in inventory.");

                return false;
            }

            if (item.Drop(gameObject))
            {
                UnregisterItem(item);

                if (item == CurrentHotbarObject)
                {
                    CurrentHotbarObject = null;
                }

                return true;
            }

            return false;
        }

        void RegisterItem(Item item)
        {
            int slot = FindFreeHotbarSlot();

            if (slot >= 0)
            {
                RegisterHotbarSlot(slot, item);
            }
        }

        void UnregisterItem(Item item)
        {
            int index = FindHotbarIndex(item);

            if (index < 0)
            {
                Debug.Log("Not in inventory.");
                return;
            }

            inventory[index] = null;
        }
    }
}
