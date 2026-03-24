using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    public string npcName { get; set; }
    public int humor { get; set; }

    public int affinity { get; set; }


    public void IncreaseMood(int amount) => humor += amount;

    public void DecreaseMood(int amount) => humor -= amount;

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            GenerateOffer();
        }
    }

    public void ReactOffer(TradeOffer offer) {

        TradeEvents.TriggerReactOffer(offer);
    }

    public bool HandleOffer(TradeOffer offer) {


        return false;
    }


    void GenerateOffer() {

        Item newItem = new Item();
        newItem.id = "001";
        newItem.name = "Lavadora";
        newItem.price = 0;
        newItem.weight = 10.5f;
        newItem.space = 2;

        NPC testNpc = new NPC();
        testNpc.npcName = "Juanito";
        testNpc.humor = 3;
        testNpc.affinity = 5;

        Trade npcTrade = new Trade{ 
            npc = testNpc,
            item = newItem
        };

        TradeEvents.TriggerTradeReceived(npcTrade);
        Debug.Log("trade received");
    }

}
