public interface IPointOnSide
{
    int point { get; }
}

public enum BuildType
{
    BuildNew,
    Upgrade
}

public enum EventType
{
    Nontrigger,
    CommunityChest,
    Chance,
    BusTicket,
    GotoJail,
    Tax,
    Surtax,
    GiftReceive,
    Auction
}