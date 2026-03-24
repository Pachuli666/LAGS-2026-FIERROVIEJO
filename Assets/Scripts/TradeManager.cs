using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance { get; private set; }

    [SerializeField]
    private TradeOffer currentOffer;

    [SerializeField]
    private PlayerInfoSO playerData;

    private List<TradeOffer> tradeHistorial { get; set; }


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        currentOffer = new TradeOffer();
    }

    private void OnEnable()
    {
        TradeEvents.OnReceiveTrade += TradeReceived;
        TradeEvents.OnSendTradeOffer += SendOffer;
    }

    private void OnDisable()
    {
        TradeEvents.OnReceiveTrade -= TradeReceived;
        TradeEvents.OnSendTradeOffer -= SendOffer;
    }

    public void LowerOffer() {
        if (currentOffer.priceOffer < 1) return;
        currentOffer.priceOffer -= 1;
        TradeEvents.TriggerUpdateOffer(currentOffer);
    }

    public void UpperOffer() {
        currentOffer.priceOffer += 1;
        TradeEvents.TriggerUpdateOffer(currentOffer);
    }

    public void AcceptOffer() { 
    
    }

    public void RejectOffer() { 
    
    }

    void TradeReceived(Trade trade) {
        currentOffer.item = trade.item;
        currentOffer.npc = trade.npc;

        UIManager.instance.OpenPanel();
    }

    void SendOffer(TradeOffer offer) {

        bool accepted = offer.npc.HandleOffer(offer);

        if (accepted)
        {
            playerData.items.Add(offer.item);
        }
        else {
            UIManager.instance.ClosePanel();
        }

    }
    
}
