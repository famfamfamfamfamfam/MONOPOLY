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
    }

    void ListenToTriggerCard(int playerIndex, EventType cardType)
    {
        switch (cardType)
        {
            case EventType.CommunityChest:
                communityCardHandler.SetActive(true);
                return;
            case EventType.Chance:
                chanceCardHandler.SetActive(true);
                return;
            case EventType.BusTicket:
                busTicketHandler.SetActive(true);
                return;
        }
    }
}