using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private PlayerInfoSO playerData;

    public string npcName;

    public int initialHumor;
    public Bindable<int> humor;

    public List<ItemSO> sellingItems;

    public int lastOffer;
    public bool firstOffer = true;

    public void IncreaseMood(int amount) => humor.Value += amount;

    public void DecreaseMood(int amount) => humor.Value = humor.Value == 1 ? 1: humor.Value -= amount;

    private void Awake()
    {
        humor.Value = initialHumor;
    }

    private void OnEnable()
    {
        TradeEvents.OnParking += GenerateOffer;
        TradeEvents.OnUpdateOffer += ReactOffer;
    }

    private void OnDisable()
    {
        TradeEvents.OnParking -= GenerateOffer;
        TradeEvents.OnUpdateOffer -= ReactOffer;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            GenerateOffer();
            //ChatarraShop.instance.PotentialEarnings = playerData.CalculateKilos(); 
        }
    }

    void ReactOffer(Trade trade,TradeOffer offer) {

        if (firstOffer) {
            lastOffer = offer.priceOffer;
            firstOffer = false;
        }

        if (offer.priceOffer > lastOffer) {
            humor.Value += 1;
        } else if (offer.priceOffer < lastOffer) {
            humor.Value -= 1;
        }

        lastOffer = offer.priceOffer;
    }

    public bool HandleOffer(TradeOffer offer) {

        var itemOffered = sellingItems.Find(i => i.item.id == offer.item.id);
        var acceptedOffer = false;
        if (itemOffered != null) {
            var basePrice = itemOffered.item.price - itemOffered.item.useFactor; // precio aceptable
            if (offer.priceOffer > basePrice)
            {
                // aceptar oferta
                acceptedOffer = true;
            }
            else if(offer.priceOffer < basePrice) {
                var offerAffinity = (humor.Value + playerData.affinity) / 2;
                if (Mathf.Abs(offerAffinity) > 5)
                {
                    // acepta oferta
                    acceptedOffer = true;
                }
                else {
                    // rechazar oferta
                    acceptedOffer = false;
                    //bajar humor
                    DecreaseMood(1);
                }
            }
            
        }

        return acceptedOffer;
    }


    void GenerateOffer() {

        Item newItem = new Item();
        newItem.id = "001";
        newItem.name = "Lavadora";
        newItem.price = 0;
        newItem.weight = 10.5f;
        newItem.space = 2;

        NPC testNpc = this;
        //testNpc.npcName = "Juanito";
        testNpc.humor.Value = 10;

        Trade npcTrade = new Trade{ 
            npc = testNpc,
            item = newItem,
            expectedOffer = 10
        };

        TradeEvents.TriggerTradeReceived(npcTrade);
        Debug.Log("trade received");
    }

}
