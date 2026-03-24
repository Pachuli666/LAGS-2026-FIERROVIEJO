using System;
using UnityEngine;

public class TradeEvents : MonoBehaviour
{
    public static event Action<Trade> OnReceiveTrade;
    public static event Action<TradeOffer> OnSendTradeOffer;
    public static event Action<TradeOffer> OnAcceptTradeOffer;
    public static event Action<TradeOffer> OnCancelTradeOffer;
    public static event Action<TradeOffer> OnReactOffer;
    public static event Action<TradeOffer> OnUpdateOffer;


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

    public static void TriggerReactOffer(TradeOffer offer) {
        OnReactOffer?.Invoke(offer);
    }

    public static void TriggerUpdateOffer(TradeOffer offer) {
        OnUpdateOffer?.Invoke(offer);
    }

}
