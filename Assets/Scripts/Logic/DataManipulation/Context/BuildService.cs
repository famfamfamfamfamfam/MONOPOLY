public class BuildService
{
    PlayerDataService playerService;
    PropertyDataService propertyService;
    public BuildService(PlayerDataService playerService, PropertyDataService propertyService)
    {
        this.playerService = playerService;
        this.propertyService = propertyService;
    }

    public void BuildNewHouse(int playerIndex, int spaceIndex)
    {
        propertyService.AddBuilding(spaceIndex, PropertyDataService.BuildType.BuildNew);
        playerService.SetCurrentCoin(playerIndex,
            -propertyService.GetCost(spaceIndex, PropertyDataService.BuildType.BuildNew));
    }

    public void UpgradeHouses(int playerIndex, int spaceIndex, BuildingRate materialBuildingsRate, BuildingRate expectedBuildingRate)
    {
        propertyService.AddBuilding(spaceIndex, PropertyDataService.BuildType.Upgrade, materialBuildingsRate, expectedBuildingRate);
        playerService.SetCurrentCoin(playerIndex,
            -propertyService.GetCost(spaceIndex, PropertyDataService.BuildType.Upgrade));
    }

}

public enum BuildingRate : byte
{
    None = 255,
    A = 3,
    B = 2,
    C = 1,
    D = 0
}