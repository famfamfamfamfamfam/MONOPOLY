public class DataInitializer
{
    public struct ConstructorParams
    {
        public GlobalConfig gameConfig;
        public SpaceGroupConfig eventSpaces;
        public CompaniesConfig companies;
        public StationsConfig stations;
        public PropertyConfig[] properties;
    }

    public DataInitializer(ConstructorParams configs, out PlayerData[] playersData, out AssetData[] assetsData, out BoardData boardData)
    {
        playersData = new PlayerData[configs.gameConfig.playerCount];
        assetsData = new AssetData[configs.gameConfig.spaceCount];

        foreach (var property in configs.properties)
        {
            assetsData[property.indexFromGoSpace] = new PropertyData(property);
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

        boardData = new BoardData(configs.gameConfig, configs.eventSpaces);
    }
}