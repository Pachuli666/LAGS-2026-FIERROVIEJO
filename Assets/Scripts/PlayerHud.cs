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


    [SerializeField]
    private TMP_Text afinityTxt;

    [SerializeField]
    private TMP_Text timeTxt;

    float time;

    private void OnEnable()
    {
        playerData.money.OnValueChanged += ChangeMoney;
        playerData.weight.OnValueChanged += ChangeWeight;
        playerData.affinity.OnValueChanged += ChangeAffinity;
    }

    //private void OnDisable()
    //{
    //    playerData.money.OnValueChanged -= ChangeMoney;
    //    playerData.weight.OnValueChanged -= ChangeWeight;
    //    playerData.affinity.OnValueChanged -= ChangeAffinity;
    //}

    private void Start()
    {
        moneyTxt.text = $"Dinero: {playerData.Money}";
        weightTxt.text = $"PESO KG: {playerData.Weight}";

    }

    private void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = $"TIEMPO :{Mathf.FloorToInt(time / 60):00}:{Mathf.FloorToInt(time % 60):00}";
    }

    public void ChangeMoney(float value) {
        moneyTxt.text = $"DINERO: {value}";
        Debug.Log($"Cambio dinero {value}");
    }

    public void ChangeWeight(float value) { 
        weightTxt.text = $"PESO KG: {value}";
    }

    public void ChangeAffinity(int value) {
        afinityTxt.text = $"AFINIDAD: {value}";
    }

}
