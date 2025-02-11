using UnityEngine;

[CreateAssetMenu(fileName = "New Tickets Config", menuName = "Scriptable Objects/Bus Tickets Config")]
public class BusTicketsConfig : ScriptableObject
{
    public const int GO_TO_JAIL = 0;
    public const int RANDOM_UTILITY_SPACE = 1;
    public const int GO_SPACE = 2;
    public const int THIRD_DIE_ROLL = 3;
    public const int AUCTION_SPACE = 4;
    public const int QUIT_FROM_JAIL = 5;
    public const int NUMBER_OF_TICKET_TYPE = 6;


    public enum InstantUseTicket : byte
    {
        GoToJail = GO_TO_JAIL,
        RandomUtilitySpace = RANDOM_UTILITY_SPACE,
        GoSpace = GO_SPACE
    }
    public enum KeepToUseTicket : byte
    {
        ThirdDieRoll = THIRD_DIE_ROLL,
        AuctionSpace = AUCTION_SPACE,
        QuitFromJail = QUIT_FROM_JAIL
    }

    public InstantUseTicket[] instantUseTickets;
    public KeepToUseTicket[] keepToUseTickets;
}