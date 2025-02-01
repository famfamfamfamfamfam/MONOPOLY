using UnityEngine;

[CreateAssetMenu(fileName = "New Tickets Config", menuName = "Scriptable Objects/Bus Tickets Config")]
public class BusTicketsConfig : CardsConfig
{
    public BusTicketAction action;

    enum InstantUseTicket : byte
    {
        GoToJail,
        RandomUtilitySpace,
        GoSpace
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

    public override void AccessTheCard(PlayerData player, PlayerData[] _, int ticketIndex)
    {
        if (ticketIndex < instantUseTickets.Length)
        {

        }
        else
        {
            action.TakeTicket(player);
        }
    }
}