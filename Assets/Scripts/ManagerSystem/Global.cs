public interface IPointOnSide
{
    int point { get; }
}

public interface IOnEvent { }

public interface IChangeCoin : IOnEvent
{
    void SetCurrentCoin(int addValue);
}

public interface ICanKeepTicket : IOnEvent
{
    void KeepTicket(int ticketIndex);
}

public interface ICanBeInJail : IOnEvent
{
    void BeInJail();
    void QuitFromJail();
}

public interface ISelfCountable
{
    void UpdateTheNumber(ref int currentCount);
}

public enum AssetType : byte
{
    Property,
    Company,
    Station
}

public enum BuildType : byte
{
    BuildNew,
    Upgrade
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

public class GlobalFieldContainer
{
    public readonly static string globalConfigPath = "ScriptableObjects/GlobalSetting";
    public readonly static string playerGeneralConfigPath = "ScriptableObjects/GeneralPlayer";
    public readonly static string companiesGroupPath = "ScriptableObjects/PurchasableSpaces/Groups/Companies";
    public readonly static string stationsGroupPath = "ScriptableObjects/PurchasableSpaces/Groups/Stations";
    public readonly static string purchasableSpacesPath = "ScriptableObjects/PurchasableSpaces/Spaces";
    public readonly static string eventSpaceGroupPath = "ScriptableObjects/EventSpaces/EventSpaces";
    public readonly static string communityChestCardsPath = "ScriptableObjects/Cards/CommunityChests";
    public readonly static string chanceCardsPath = "ScriptableObjects/Cards/Chances";
    public readonly static string busTicketsPath = "ScriptableObjects/Cards/BusTickets";
    public readonly static string taxSpacePath = "ScriptableObjects/EventSpaces/Spaces/TaxSpace";
    public readonly static string surtaxSpacePath = "ScriptableObjects/EventSpaces/Spaces/SurtaxSpace.asset";

    public readonly static int allTicketType = 6;
    public readonly static int allKeepToUseTicket = 5;
    public const float RESIZE_THRESHOLD = 75 / 100;
}