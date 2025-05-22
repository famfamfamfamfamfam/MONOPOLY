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

    public DataInitializer(ConstructorParams configs, out PlayerData[] playersData, out AssetData[] assetsData, out BoardData boardData)
    {
        playersData = new PlayerData[configs.gameConfig.playerCount];
        for (int i = 0; i < playersData.Length; i++)
        {
            playersData[i] = new PlayerData(configs.playersConfig);
        }

        assetsData = new AssetData[configs.gameConfig.spaceCount];
        for (int i = 0; i < configs.properties.Length; i++)
        {
            if (configs.properties[i] == null)
            {
                continue;
            }
            assetsData[i] = new PropertyData(configs.properties[i]);
        }
        for (int i = 0; i < assetsData.Length; i++)
        {
            if (configs.companies.spacesIndices.Contains(i))
            {
                assetsData[i] = new CompanyData();
            }
            else if (configs.stations.spacesIndices.Contains(i))
            {
                assetsData[i] = new StationData(configs.stations);
            }
        }

        boardData = new BoardData(configs.gameConfig, configs.eventSpaces, configs.busTicketsConfig);
    }
}