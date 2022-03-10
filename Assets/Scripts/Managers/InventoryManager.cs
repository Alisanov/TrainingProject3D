using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    private Dictionary<string, int> _items;
    public string equippedItem { get; private set; }

    private NetworkService _network;
    public void Startup(NetworkService service)
    {
        Debug.Log("InventoryManager starting...");

        _items = new Dictionary<string, int>();

        _network = service;

        status = ManagerStatus.Started;
    }

    private void DisplayItems()
    {
        string itemsDisplay = "Items: ";
        foreach (KeyValuePair<string, int> item in _items)
            itemsDisplay += item.Key + "(" + item.Value + ")";
        Debug.Log(itemsDisplay);
    }

    public void AddItem(string name)
    {
        if (_items.ContainsKey(name))
            _items[name] += 1;
        else
            _items[name] = 1;
        DisplayItems();
    }

    public List<string> GetItemsList()
    {
        List<string> list = new List<string>(_items.Keys);
        return list;
    }

    public int GetItemCount(string name)
    {
        if (_items.ContainsKey(name))
            return _items[name];
        return 0;
    }

    public bool EquipItem(string name)
    {
        if(_items.ContainsKey(name) && equippedItem != name)
        {
            equippedItem = name;
            Debug.Log("Equipped " + name);
            return true;
        }

        equippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }

    public bool ConsumeItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name]--;
            if (_items[name] == 0)
                _items.Remove(name);
        }
        else
        {
            Debug.Log("cannot consume " + name);
            return false;
        }

        DisplayItems();
        return true;
    }

    public void UpdateData(Dictionary<string, int> items)
    {
        _items = items;
    }

    public Dictionary<string, int> GetData()
    {
        return _items;
    }
}
