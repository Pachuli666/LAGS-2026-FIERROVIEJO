using TMPro;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{

    [SerializeField]
    private PlayerInfoSO playerData;

    [SerializeField]
    private TMP_Text moneyTxt;

    private void OnEnable()
    {
        playerData.money.OnValueChanged += ChangeMoney;
    }

    private void Start()
    {
        moneyTxt.text = $"Dinero: {playerData.money.Value}";
    }

    public void ChangeMoney(float value) {
        moneyTxt.text = $"Dinero: {value}";
        Debug.Log("Dinero Cambio");
    }

}
