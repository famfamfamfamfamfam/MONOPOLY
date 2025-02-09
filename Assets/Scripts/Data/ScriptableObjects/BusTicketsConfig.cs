using UnityEngine;

[CreateAssetMenu(fileName = "New Tickets Config", menuName = "Scriptable Objects/Bus Tickets Config")]
public class BusTicketsConfig : ScriptableObject
{
    public enum InstantUseTicket : byte
    {
        GoToJail = 0,
        RandomUtilitySpace = 1,
        GoSpace = 2
    }
    public enum KeepToUseTicket : byte
    {
        ThirdDieRoll = 3,
        AuctionSpace = 4,
        QuitFromJail = 5
    }

    public InstantUseTicket[] instantUseTickets;
    public KeepToUseTicket[] keepToUseTickets;
}