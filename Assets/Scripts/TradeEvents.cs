using System;
using UnityEngine;

public class TradeEvents : MonoBehaviour
{
    public static event Action OnParking;
    public static event Action<Trade> OnReceiveTrade;
    public static event Action<TradeOffer> OnSendTradeOffer;
    public static event Action<TradeOffer> OnAcceptTradeOffer;
    public static event Action<TradeOffer> OnCancelTradeOffer;
    public static event Action<Trade, TradeOffer> OnUpdateOffer;


    public static void TriggerTradeReceived(Trade trade) {
        OnReceiveTrade?.Invoke(trade);
    }

    public static void TriggerSendOffer(TradeOffer offer) {
        OnAcceptTradeOffer?.Invoke(offer);
    }

    public static void TriggerAcceptOffer(TradeOffer offer) {
        OnAcceptTradeOffer?.Invoke(offer);
    }

    public static void TriggerCancelOffer(TradeOffer offer) {
        OnCancelTradeOffer?.Invoke(offer);
    }

    public static void TriggerUpdateOffer(Trade trade, TradeOffer offer) {
        OnUpdateOffer?.Invoke(trade, offer);
    }

    public static void TriggerOnParking()
    {
        OnParking?.Invoke();
    }

}
