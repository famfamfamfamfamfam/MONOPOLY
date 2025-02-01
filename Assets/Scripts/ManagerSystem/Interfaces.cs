public interface IPointOnSide
{
    int point { get; }
}

public enum BuildType : byte
{
    BuildNew,
    Upgrade
}

public enum EventType : byte
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