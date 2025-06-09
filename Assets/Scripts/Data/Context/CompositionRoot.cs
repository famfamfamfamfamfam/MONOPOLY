using System.Collections.Generic;
using UnityEngine;

public class CompositionRoot : MonoBehaviour, INeedRefRuntime
{
    [SerializeField]
    PlayerManager playerManager;
    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    DiceRoller diceRoller;
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
    TriggerSpaceService.ConstructorParams inputForTriggerService;
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
        playerService = new PlayerDataService(dataOutputs.playersData, configs.playersConfig);

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
            moveAction = (position, index) =>
            {
                Debug.Log($"Move to the {index} space at {position}");
                playerManager.MovePlayerTo(index, position);
            },
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

        if (inputForTriggerService == null)
        {
            inputForTriggerService = new TriggerSpaceService.ConstructorParams();
        }
        inputForTriggerService.eventSpaces = configs.eventSpaceGroup;
        inputForTriggerService.companies = configs.companiesConfig;
        inputForTriggerService.stations = configs.stationsConfig;
        inputForTriggerService.properties = configs.propertySpaces;
        inputForTriggerService.taxConfig = configs.taxConfig;
        inputForTriggerService.surtaxConfig = configs.surtaxConfig;
        inputForTriggerService.moveToJail = (index, position) =>
        {
            Debug.Log($"Move to jail at {position}");
            playerManager.MovePlayerTo(index, position);
        };
        inputForTriggerService.propertyService = propertyService;
        inputForTriggerService.companyService = companyService;
        inputForTriggerService.stationService = stationService;
        inputForTriggerService.assetService = assetService;
        inputForTriggerService.boardService = boardService;
        inputForTriggerService.playerService = this.playerService;

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
            diceRoller = this.diceRoller,
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

    public void Init(RuntimeRefWrapper refProvider)
    {
        if (inputForTriggerService == null)
        {
            inputForTriggerService = new TriggerSpaceService.ConstructorParams();
        }
        if (refProvider.GetReference(out List<JailPositionProperty> listHasOneElement))
        {
            inputForTriggerService.theJailPosition = listHasOneElement[0].position;
        }
        else
        {
            Debug.LogError("Cannot get jail position");
        }
    }


    private void OnDisable()
    {
        configInitializer.UnloadAssets(configs);
        Resources.UnloadUnusedAssets();
    }
}
