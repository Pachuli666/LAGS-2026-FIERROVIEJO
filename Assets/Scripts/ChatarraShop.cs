using TMPro;
using UnityEngine;

public class ChatarraShop : MonoBehaviour
{
    public static ChatarraShop instance;

    [SerializeField]
    private PlayerInfoSO playerData;

    public float priceByKilo = 0.30f;

    [SerializeField]
    private Bindable<float> potentialEarnings;

    [SerializeField]
    private TMP_Text earningsTxt;

    [SerializeField]
    private TMP_Text weightTxt;


    public float PotentialEarnings { get { return potentialEarnings.Value;  } set { potentialEarnings.Value = value; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        potentialEarnings.OnValueChanged += UpdateEarnings;
    }


    public void Sell() { 
    
    }

    public void Purchase() { 
        
    }

    float Calculate() {
        var earnings = playerData.CalculateKilos() * priceByKilo;
        return earnings;
    }

    public void Pay() {
        if (PotentialEarnings > 0) { 
            playerData.AddMoney(PotentialEarnings);
            PotentialEarnings = 0;
            playerData.items.Clear();
            UpdateEarnings(0);
        }
    }

    void UpdateEarnings(float earnings) {
        UpdateWeight();
        var earning = Calculate();
        PotentialEarnings = earning;
        earningsTxt.text = $"Ganancia: {earning} Pesos";
    }
    
    void UpdateWeight() {
        weightTxt.text = $"Peso: {playerData.CalculateKilos()} kl";
    }

}
