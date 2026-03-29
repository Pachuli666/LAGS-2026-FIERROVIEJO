using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerInfoSO: ScriptableObject
{

    public Bindable<float> money;
    public Bindable<float> weight;
    public int affinity;
    public int maxInventorySpace;
    public List<Item> items;

    public float Money { get { return money.Value; } set { money.Value = value;  } }

    public float Weight { get { return weight.Value; } set { weight.Value = value; } }


    public bool CanAfford(int price) => money.Value >= price;
    public bool HasInventorySpace() => items.Count < maxInventorySpace;
    public bool HasAffinity(float requiredAffinity) => affinity >= requiredAffinity;
    public void DeductMoney(float amount) => Money -= amount;
    public void AddMoney(float amount) => Money += amount;
    public void AddItem(Item item) => items.Add(item);

    public float CalculateKilos() => items.Sum(i => i.weight);
}
