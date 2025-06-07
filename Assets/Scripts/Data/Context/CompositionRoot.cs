using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField]
    PlayerManager playerManager;
    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    CommunityChestHandler communityChestHandler;
    [SerializeField]
    ChanceCardHandler chanceCardHandler;
    [SerializeField]
    BusTickerHandler busTickerHandler;
    [SerializeField]
    GameObject[] needDriverUI;

    ConfigInitializer configInitializer;
    ConfigInitializer.ConstructorParams configs;
    TriggerSpaceService triggerSpaceService;
    PlayerDataService playerService;
    void Awake()
    {
        #region Create Configs
        configInitializer = new ConfigInitializer(out configs);
        #endregion
        #region Create Data
        DataInitializer.ConstructorParams inputForData = new DataInitializer.ConstructorParams()
        {
            gameConfig = configs.gameConfig,
            playersConfig = configs.playersConfig,
            eventSpaces = configs.eventSpaceGroup,
            busTicketsConfig = configs.busTickets,
            companies = configs.companiesConfig,
            stations = configs.stationsConfig,
            properties = configs.propertySpaces
        };

        DataInitializer dataInitializer = new DataInitializer(inputForData, out DataInitializer.ConstructorOuputs dataOutputs);
        #endregion
        #region Create Services
        playerService = new PlayerDataService(dataOutputs.playersData);

        BoardDataService boardService = new BoardDataService(dataOutputs.boardData);

        CommunityChestService communityService = new CommunityChestService(configs.communityCards, configs.gameConfig, playerService);

        BusTicketService.ConstructorParams inputForBusService = new BusTicketService.ConstructorParams()
        {
            config = configs.busTickets,
            companies = configs.companiesConfig,
            stations = configs.stationsConfig,
            goSpace = configs.goSpace,
            auctionSpace = configs.auctionSpace,
            goToJailSpace = configs.gotoJailSpace,
            prison = configs.prisonVisitSpace,
            playerService = playerService,
            boardService = boardService,
            moveAction = (position, index) => Debug.Log($"Move to the {index} space at {position}"),
            rollThirdDieAndStepAction = () => Debug.Log("Roll third die and step")
        };
        BusTicketService busService = new BusTicketService(inputForBusService);

        ChanceService.ConstructorParams inputForChanceService = new ChanceService.ConstructorParams()
        {
            config = configs.chanceCards,
            gameConfig = configs.gameConfig,
            service = playerService,
            triggerCommunityCard = index => communityService.TriggerACard(index),
            triggerBusTicket = index => busService.TriggerACard(index)
        };
        ChanceService chanceService = new ChanceService(inputForChanceService);

        AssetAccessor assetService = new AssetAccessor(dataOutputs.assetsData);
        PropertyDataService propertyService = new PropertyDataService(configs.propertySpaces, dataOutputs.assetsData);
        CompanyDataService companyService = new CompanyDataService(configs.companiesConfig, dataOutputs.assetsData);
        StationDataService stationService = new StationDataService(configs.stationsConfig);
        TriggerSpaceService.ConstructorParams inputForTriggerService = new TriggerSpaceService.ConstructorParams()
        {
            eventSpaces = configs.eventSpaceGroup,
            companies = configs.companiesConfig,
            stations = configs.stationsConfig,
            properties = configs.propertySpaces,
            taxConfig = configs.taxConfig,
            surtaxConfig = configs.surtaxConfig,
            theJailPosition = Vector3.one,
            moveToJail = position => Debug.Log($"Move to jail at {position}"),
            propertyService = propertyService,
            companyService = companyService,
            stationService = stationService,
            assetService = assetService,
            boardService = boardService,
            playerService = playerService
        };
        triggerSpaceService = new TriggerSpaceService(inputForTriggerService);
        #endregion

        DataManager.instance.Init(configs, dataOutputs, triggerSpaceService);
        playerManager.Init(configs, playerService, triggerSpaceService);
        inputManager.Init(triggerSpaceService, dataOutputs.commonData);
        communityChestHandler.Init(communityService);
        chanceCardHandler.Init(chanceService);
        busTickerHandler.Init(busService);

        Driver.ConstructorParams driverInput = new Driver.ConstructorParams()
        {
            manager = DataManager.instance,
            commonData = dataOutputs.commonData,
            playerService = this.playerService,
            triggerSpaceService = this.triggerSpaceService,
            busTicketService = busService,
            playersInitialCoin = configs.playersConfig.initialCoin,
            assetAccessor = assetService,
            properties = configs.propertySpaces
        };

        Driver driver = new Driver(driverInput);
        for (int i = 0; i < needDriverUI.Length; i++)
        {
            if (needDriverUI[i].TryGetComponent(out INeedDriver needer))
            {
                needer.driver = driver;
            }
        }
    }

    private void OnDisable()
    {
        configInitializer.UnloadAssets(configs);
        Resources.UnloadUnusedAssets();
    }
}
