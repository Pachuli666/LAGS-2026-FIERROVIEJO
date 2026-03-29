using TMPro;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{

    [SerializeField]
    private PlayerInfoSO playerData;

    [SerializeField]
    private TMP_Text moneyTxt;

    [SerializeField]
    private TMP_Text weightTxt;

    private void OnEnable()
    {
        playerData.money.OnValueChanged += ChangeMoney;
        playerData.weight.OnValueChanged += ChangeWeight;
    }

    private void Start()
    {
        moneyTxt.text = $"Dinero: {playerData.Money}";
        weightTxt.text = $"PESO KG: {playerData.Weight}";

    }

    public void ChangeMoney(float value) {
        moneyTxt.text = $"DINERO: {value}";
    }

    public void ChangeWeight(float value) { 
        weightTxt.text = $"PESO KG: {value}";
    
    }

}
