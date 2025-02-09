public class DataContainer
{
    PlayerData[] playersData;
    AssetData[] assetsData;
    public DataContainer(PlayerData[] playersData, AssetData[] assetsData)
    {
        this.playersData = playersData;
        this.assetsData = assetsData;
    }

    public PlayerData GetPlayerData(int index)
    {
        return playersData[index];
    }

    public AssetData GetAssetsData(int index)
    {
        return assetsData[index];
    }
}