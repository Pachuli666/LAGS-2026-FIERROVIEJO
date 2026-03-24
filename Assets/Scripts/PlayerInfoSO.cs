using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerInfoSO: ScriptableObject
{

    public Bindable<int> money;
    public int affinity;
    public int maxInventorySpace;
    public List<Item> items;

    public int Money { get { return money.Value; } set { money.Value = value;  } }

    public bool CanAfford(int price) => money.Value >= price;
    public bool HasInventorySpace() => items.Count < maxInventorySpace;
    public bool HasAffinity(float requiredAffinity) => affinity >= requiredAffinity;
    public void DeductMoney(int amount) => money.Value -= amount;
    public void AddItem(Item item) => items.Add(item);
}
