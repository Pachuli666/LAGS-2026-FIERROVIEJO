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
        TradeEvents.OnUpdateOffer += Refresh;
    }

    private void OnDisable()
    {
        TradeEvents.OnReceiveTrade -= Populate;
        TradeEvents.OnUpdateOffer -= Refresh;
    }

    void Populate(Trade trade) {
        offerText.text = trade.expectedOffer.ToString();
        npcNameTxt.text = trade.npc.npcName;
        npcHumor.text = trade.npc.humor.Value.ToString();
    }

    void Refresh(Trade trade, TradeOffer offer) {
        npcHumor.text = offer.npc.humor.Value.ToString();
        offerText.text = offer.priceOffer.ToString();
    }

    public void Decrease() {
        TradeManager.Instance.LowerOffer();
        Debug.Log("llama?");
    }

    public void Increase() {
        TradeManager.Instance.UpperOffer();
    }


}
