using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject rollHandler;
    [SerializeField]
    GameObject chanceCardHandler;
    [SerializeField]
    GameObject communityCardHandler;
    [SerializeField]
    GameObject busTicketHandler;

    public void Init(TriggerSpaceService triggerSv, GameData data)
    {
        triggerSv.waitForTriggerCard = ListenToTriggerCard;

        DiceRoller roller = rollHandler.GetComponent<DiceRoller>();
        CommunityChestHandler community = communityCardHandler.GetComponent<CommunityChestHandler>();
        ChanceCardHandler chance = chanceCardHandler.GetComponent<ChanceCardHandler>();
        BusTickerHandler bus = busTicketHandler.GetComponent<BusTickerHandler>();

        community.SetGamerIndex(data.gamerPlayIndex);
        chance.SetGamerIndex(data.gamerPlayIndex);
        bus.SetGamerIndex(data.gamerPlayIndex);

        triggerSv.onAlreadyTriggeredSpace = roller.ActiveRoll;
        community.onFinishCardTrigger = roller.ActiveRoll;
        chance.onFinishCardTrigger = roller.ActiveRoll;
        bus.onFinishCardTrigger = roller.ActiveRoll;

    }

    void ListenToTriggerCard(int playerIndex, EventType cardType)
    {
        switch (cardType)
        {
            case EventType.CommunityChest:
                DeactiveInTheRest(chanceCardHandler, busTicketHandler);
                communityCardHandler.SetActive(true);
                return;
            case EventType.Chance:
                DeactiveInTheRest(communityCardHandler, busTicketHandler);
                chanceCardHandler.SetActive(true);
                return;
            case EventType.BusTicket:
                DeactiveInTheRest(communityCardHandler, chanceCardHandler);
                busTicketHandler.SetActive(true);
                return;
        }
    }

    void DeactiveInTheRest(params GameObject[] theRest)
    {
        for (int i = 0; i < theRest.Length; i++)
        {
            if (theRest[i].activeSelf)
            {
                theRest[i].SetActive(false);
                return;
            }
        }
    }
}