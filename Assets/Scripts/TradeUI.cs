using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text npcNameTxt;

    [SerializeField]
    private TMP_Text offerText;

    [SerializeField]
    private RawImage npcImg;

    [SerializeField]
    private Image itemImg;

    [SerializeField]
    private TMP_Text npcHumor;

    [SerializeField]
    private RawImage npcHumorImg;


    private void OnEnable()
    {
        TradeEvents.OnReceiveTrade += Populate;
        TradeEvents.OnReactOffer += Refresh;
        TradeEvents.OnUpdateOffer += Refresh;
    }

    private void OnDisable()
    {
        TradeEvents.OnReceiveTrade -= Populate;
        TradeEvents.OnReactOffer -= Refresh;
        TradeEvents.OnUpdateOffer -= Refresh;
    }

    void Populate(Trade trade) {
        offerText.text = trade.item.price.ToString();
        npcNameTxt.text = trade.npc.npcName;
        npcHumor.text = trade.npc.humor.ToString();
    }

    void Refresh(TradeOffer offer) {
        offerText.text = offer.priceOffer.ToString();
    }

    public void Decrease() {
        TradeManager.Instance.LowerOffer();
    }

    public void Increase() {
        TradeManager.Instance.UpperOffer();
    }


}
