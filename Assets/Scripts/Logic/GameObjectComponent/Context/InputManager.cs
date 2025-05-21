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

    public void Init(TriggerSpaceService triggerSv)
    {
        triggerSv.waitForTriggerCard = ListenToTriggerCard;

        DiceRoller roller = rollHandler.GetComponent<DiceRoller>();
        communityCardHandler.GetComponent<CommunityChestHandler>()
            .onFinishCardTrigger = () => roller.ActiveRoll(true);
        chanceCardHandler.GetComponent<ChanceCardHandler>()
            .onFinishCardTrigger = () => roller.ActiveRoll(true);
        busTicketHandler.GetComponent<BusTickerHandler>()
            .onFinishCardTrigger = () => roller.ActiveRoll(true);
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