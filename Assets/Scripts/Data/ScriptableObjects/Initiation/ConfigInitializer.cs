using UnityEngine;

public class ConfigInitializer
{
    readonly string globalConfigPath = "ScriptableObjects/GlobalSetting";
    readonly string playerGeneralConfigPath = "ScriptableObjects/GeneralPlayer";
    readonly string companyGroupPath = "ScriptableObjects/PurchasableSpaces/Groups/Companies";
    readonly string stationGroupPath = "ScriptableObjects/PurchasableSpaces/Groups/Stations";
    readonly string purchasableSpacesPath = "ScriptableObjects/PurchasableSpaces/Spaces";
    readonly string eventSpaceGroupPath = "ScriptableObjects/EventSpaces/EventSpaces";
    readonly string communityChestCardsPath = "ScriptableObjects/Cards/CommunityChests";
    readonly string chanceCardsPath = "ScriptableObjects/Cards/Chances";
    readonly string busTicketsPath = "ScriptableObjects/Cards/BusTickets";
    readonly string taxSpacePath = "ScriptableObjects/EventSpaces/Spaces/TaxSpace";
    readonly string surtaxSpacePath = "ScriptableObjects/EventSpaces/Spaces/SurtaxSpace";
    readonly string goSpacePath = "ScriptableObjects/EventSpaces/Spaces/Go";
    readonly string gotoJailSpacePath = "ScriptableObjects/EventSpaces/Spaces/GoToJail";
    readonly string prisonVisitSpacePath = "ScriptableObjects/EventSpaces/Spaces/PrisonVisit";
    readonly string auctionSpacePath = "ScriptableObjects/EventSpaces/Spaces/AuctionSpace";
    
    public struct ConstructorParams
    {
        public GlobalConfig gameConfig;
        public PlayerGeneralConfig playersConfig;
        public CompaniesConfig companiesConfig;
        public StationsConfig stationsConfig;
        public SpaceGroupConfig eventSpaceGroup;
        public CommunityChestsConfig communityCards;
        public ChancesConfig chanceCards;
        public BusTicketsConfig busTickets;
        public TaxConfig taxConfig;
        public TaxConfig surtaxConfig;
        public SpaceConfig goSpace;
        public SpaceConfig gotoJailSpace;
        public SpaceConfig prisonVisitSpace;
        public SpaceConfig auctionSpace;
        public PropertyConfig[] propertySpaces;
    }

    public ConfigInitializer(out ConstructorParams outputs)
    {
        outputs = new ConstructorParams()
        {
            gameConfig = Resources.Load<GlobalConfig>(globalConfigPath),
            playersConfig = Resources.Load<PlayerGeneralConfig>(playerGeneralConfigPath),

            companiesConfig = Resources.Load<CompaniesConfig>(companyGroupPath),
            stationsConfig = Resources.Load<StationsConfig>(stationGroupPath),

            eventSpaceGroup = Resources.Load<SpaceGroupConfig>(eventSpaceGroupPath),
            taxConfig = Resources.Load<TaxConfig>(taxSpacePath),
            surtaxConfig = Resources.Load<TaxConfig>(surtaxSpacePath),
            goSpace = Resources.Load<SpaceConfig>(goSpacePath),
            gotoJailSpace = Resources.Load<SpaceConfig>(gotoJailSpacePath),
            prisonVisitSpace = Resources.Load<SpaceConfig>(prisonVisitSpacePath),
            auctionSpace = Resources.Load<SpaceConfig>(auctionSpacePath),

            communityCards = Resources.Load<CommunityChestsConfig>(communityChestCardsPath),
            chanceCards = Resources.Load<ChancesConfig>(chanceCardsPath),
            busTickets = Resources.Load<BusTicketsConfig>(busTicketsPath),
        };

        PropertyConfig[] temp = Resources.LoadAll<PropertyConfig>(purchasableSpacesPath);
        outputs.propertySpaces = new PropertyConfig[outputs.gameConfig.spaceCount];
        foreach (var property in temp)
        {
            outputs.propertySpaces[property.indexFromGoSpace] = property;
        }
    }

    public void UnloadAssets(ConstructorParams inputs)
    {
        Resources.UnloadAsset(inputs.gameConfig);
        Resources.UnloadAsset(inputs.playersConfig);
        Resources.UnloadAsset(inputs.companiesConfig);
        Resources.UnloadAsset(inputs.stationsConfig);
        Resources.UnloadAsset(inputs.eventSpaceGroup);
        Resources.UnloadAsset(inputs.taxConfig);
        Resources.UnloadAsset(inputs.surtaxConfig);
        Resources.UnloadAsset(inputs.goSpace);
        Resources.UnloadAsset(inputs.gotoJailSpace);
        Resources.UnloadAsset(inputs.prisonVisitSpace);
        Resources.UnloadAsset(inputs.auctionSpace);
        Resources.UnloadAsset(inputs.communityCards);
        Resources.UnloadAsset(inputs.chanceCards);
        Resources.UnloadAsset(inputs.busTickets);
        foreach (var property in inputs.propertySpaces)
        {
            Resources.UnloadAsset(property);
        }
    }
}