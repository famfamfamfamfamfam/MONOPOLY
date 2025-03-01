using UnityEngine;

public class DataManager : MonoBehaviour
{
    ConfigInitializer configInitializer;
    ConfigInitializer.ConstructorParams configs;
    TriggerSpaceService triggerSpaceService;
    PlayerDataService playerService;
    PlayerData[] playersData;
    AssetData[] assetsData;
    void Start()
    {
        #region Create Configs
        configInitializer = new ConfigInitializer(out configs);
        #endregion
        #region Create Datas
        DataInitializer.ConstructorParams inputForData = new DataInitializer.ConstructorParams()
        {
            gameConfig = configs.gameConfig,
            playersConfig = configs.playersConfig,
            eventSpaces = configs.eventSpaceGroup,
            companies = configs.companiesConfig,
            stations = configs.stationsConfig,
            properties = configs.propertySpaces
        };
        DataInitializer dataInitializer = new DataInitializer(inputForData, out playersData, out assetsData, out BoardData boardData);
        #endregion
        #region Create Services
        playerService = new PlayerDataService(playersData);

        BoardDataService boardService = new BoardDataService(boardData);

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
                
        AssetAccessor assetService = new AssetAccessor(assetsData);
        PropertyDataService propertyService = new PropertyDataService(configs.propertySpaces, assetsData);
        CompanyDataService companyService = new CompanyDataService(configs.companiesConfig, assetsData);
        StationDataService stationService = new StationDataService(configs.stationsConfig);
        TriggerSpaceService.ConstructorParams inputForTriggerService = new TriggerSpaceService.ConstructorParams()
        {
            eventSpaces = configs.eventSpaceGroup,
            companies = configs.companiesConfig,
            stations = configs.stationsConfig,
            taxConfig = configs.taxConfig,
            surtaxConfig = configs.surtaxConfig,
            theJailPosition = Vector3.one,
            moveToJail = position => Debug.Log($"Move to jail at {position}"),
            communityService = communityService,
            chanceService = chanceService,
            busService = busService,
            propertyService = propertyService,
            companyService = companyService,
            stationService = stationService,
            assetService = assetService,
            boardService = boardService,
            playerService = playerService
        };
        triggerSpaceService = new TriggerSpaceService(inputForTriggerService);
        #endregion
    }

    public void TriggerSpace(int playerIndex, ref int spaceIndex)
    {
        if (spaceIndex == configs.gameConfig.spaceCount)
        {
            spaceIndex = 0;
        }
        triggerSpaceService.TriggerSpace(playerIndex, spaceIndex);
    }

    int curPosIndex;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            curPosIndex++;
            Debug.Log(curPosIndex);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerSpace(0, ref curPosIndex);
        }
    }

    private void OnDisable()
    {
        configInitializer.UnloadAssets(configs);
    }
}
