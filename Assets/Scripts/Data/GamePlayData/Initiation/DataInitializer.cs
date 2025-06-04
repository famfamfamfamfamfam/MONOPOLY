public class DataInitializer
{
    public struct ConstructorParams
    {
        public GlobalConfig gameConfig;
        public PlayerGeneralConfig playersConfig;
        public SpaceGroupConfig eventSpaces;
        public BusTicketsConfig busTicketsConfig;
        public CompaniesConfig companies;
        public StationsConfig stations;
        public PropertyConfig[] properties;
    }

    public struct ConstructorOuputs
    {
        public GameData commonData;
        public PlayerData[] playersData;
        public AssetData[] assetsData;
        public BoardData boardData;
    }

    public DataInitializer(ConstructorParams configs, out ConstructorOuputs outputs)
    {
        outputs = new ConstructorOuputs();
        outputs.commonData = new GameData();
        outputs.playersData = new PlayerData[configs.gameConfig.playerCount];
        for (int i = 0; i < outputs.playersData.Length; i++)
        {
            outputs.playersData[i] = new PlayerData(configs.playersConfig);
        }

        outputs.assetsData = new AssetData[configs.gameConfig.spaceCount];
        for (int i = 0; i < configs.properties.Length; i++)
        {
            if (configs.properties[i] == null)
            {
                continue;
            }
            outputs.assetsData[i] = new PropertyData(configs.properties[i]);
        }
        for (int i = 0; i < outputs.assetsData.Length; i++)
        {
            if (configs.companies.spacesIndices.Contains(i))
            {
                outputs.assetsData[i] = new CompanyData();
            }
            else if (configs.stations.spacesIndices.Contains(i))
            {
                outputs.assetsData[i] = new StationData(configs.stations);
            }
        }

        outputs.boardData = new BoardData(configs.gameConfig, configs.eventSpaces, configs.busTicketsConfig);
    }
}