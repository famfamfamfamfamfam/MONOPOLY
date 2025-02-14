using UnityEngine;

[CreateAssetMenu(fileName = "New Event Space", menuName = "Scriptable Objects/Event Space Config")]
public class EventSpaceConfig : SpaceConfig
{
    public EventType eventType;
}

public enum EventType : byte
{
    Nontrigger = 0,
    CommunityChest = 1,
    Chance = 2,
    BusTicket = 3,
    GotoJail = 4,
    Tax = 5,
    Surtax = 6,
    GiftReceive = 7,
    Auction = 8
}