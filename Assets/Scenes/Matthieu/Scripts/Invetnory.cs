using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public int currentItemIndex = 0;

    void Start()
    {
        foreach (var item in items)
        {
            item.SetActive(false);
        }
        EquipItem(currentItemIndex);
    }

    void Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i < items.Count)
                {
                    UnequipItem(currentItemIndex);
                    currentItemIndex = i;
                    EquipItem(currentItemIndex);
                }
            }
        }
    }

    public void AddEquippedItem(GameObject item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            item.SetActive(false);
        }
    }

    public void EquipItem(int index)
    {
        items[index].SetActive(true);
        IEquippable equippable = items[index].GetComponent<IEquippable>();
        if (equippable != null)
            equippable.Equip();
    }

    public void UnequipItem(int index)
    {
        items[index].SetActive(false);
        IEquippable equippable = items[index].GetComponent<IEquippable>();
        if (equippable != null)
            equippable.Unequip();
    }

    public void RemoveEquippedItem(GameObject item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }
}