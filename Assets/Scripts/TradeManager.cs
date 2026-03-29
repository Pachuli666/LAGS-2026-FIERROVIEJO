using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum TradeStatus { 
    NONE,
    ACCEPTED,
    REJECTED
}

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance { get; private set; }

    [SerializeField]
    private TradeOffer currentOffer;

    [SerializeField]
    private Trade currentTrade;

    [SerializeField]
    private PlayerInfoSO playerData;

    private List<TradeOffer> tradeHistorial { get; set; }

    public TradeStatus status = TradeStatus.NONE;

    public float initialMoney;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        currentOffer = new TradeOffer();
        InitData();
    }

    private void OnEnable()
    {
        TradeEvents.OnReceiveTrade += TradeReceived;
    }

    private void OnDisable()
    {
        TradeEvents.OnReceiveTrade -= TradeReceived;
    }

    public void LowerOffer() {
        if (currentOffer.priceOffer < 1) return;
        currentOffer.priceOffer -= 1;
        
        TradeEvents.TriggerUpdateOffer(currentTrade, currentOffer);
    }

    public void UpperOffer() {
        if (currentOffer.priceOffer <= playerData.Money) { 
            currentOffer.priceOffer += 1;
            TradeEvents.TriggerUpdateOffer(currentTrade, currentOffer);
        }
    }

    public void OfferAccepted() {
        if (playerData.CanAfford(currentOffer.priceOffer)) {
            // Comprar articulo
            playerData.DeductMoney(currentOffer.priceOffer);

        }
    }

    public void OfferRejected() {
        UIManager.instance.ClosePanel();
    }

    void TradeReceived(Trade trade) {
        currentTrade = trade;
        currentOffer.item = trade.item;
        currentOffer.npc = trade.npc;
        currentOffer.priceOffer = trade.expectedOffer;
        UISwitch.Instance.ShowTradeUI();
    }

    public void TryOffer() {

        bool accepted = currentOffer.npc.HandleOffer(currentOffer);

        if (accepted)
        {
            // llamar a OfferAccepted
            status = TradeStatus.ACCEPTED;
            Debug.Log("NPC acepto tu oferta");
        }
        else {
            // llamar a offerRejected
            status = TradeStatus.REJECTED;
            Debug.Log("NPC no le gusto tu oferta");
        }

    }

    public void SendOffer() {

        bool finalChoice = currentOffer.npc.HandleOffer(currentOffer);
        // Oferta oficial no se puede cancelar
        if (finalChoice && playerData.CanAfford(currentOffer.priceOffer))
        {
            EndTransaccion();
            TradeEvents.TriggerAcceptOffer(currentOffer);
        }
        else {
            currentOffer.npc.DecreaseMood(1);
            TradeEvents.TriggerCancelOffer(currentOffer);
        }   
    }

    void InitData() {
        playerData.Money = initialMoney;
    }

    void EndTransaccion() {

        playerData.DeductMoney(currentOffer.priceOffer);
        playerData.AddItem(currentOffer.item);
        playerData.CalculateKilos();
    }
    
}
