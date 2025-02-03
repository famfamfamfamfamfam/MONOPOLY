using UnityEngine;

[CreateAssetMenu(fileName = "New Tickets Config", menuName = "Scriptable Objects/Bus Tickets Config")]
public class BusTicketsConfig : CardsConfig
{
    public BusTicketAction action;

    enum InstantUseTicket : byte
    {
        GoToJail = 0,
        RandomUtilitySpace = 1,
        GoSpace = 2
    }
    enum KeepToUseTicket : byte
    {
        ThirdDieRoll = 3,
        AuctionSpace = 4,
        QuitFromJail = 5
    }
    [SerializeField]
    InstantUseTicket[] instantUseTickets;
    [SerializeField]
    KeepToUseTicket[] keepToUseTickets;

    public override void AccessTheCard(IOnEvent player, IChangeCoin[] _, int ticketIndex)
    {
        if (ticketIndex < instantUseTickets.Length)
        {
            action.TriggerInstantUseTicket(player, (int)instantUseTickets[ticketIndex]);
        }
        else
        {
            action.GiveTicket(player as ICanKeepTicket, ticketIndex,
                (int)keepToUseTickets[ticketIndex % instantUseTickets.Length]);
        }
    }
}