using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerInfoSO: ScriptableObject
{
    public int money;
    public int affinity;
    public int maxInventorySpace;
    public List<Item> items;

    public bool CanAfford(int price) => money >= price;
    public bool HasInventorySpace() => items.Count < maxInventorySpace;
    public bool HasAffinity(float requiredAffinity) => affinity >= requiredAffinity;
    public void DeductMoney(int amount) => money -= amount;
    public void AddItem(Item item) => items.Add(item);
}
