using System;

public class BuildService
{
    PlayerDataService playerService;
    PropertyDataService propertyService;
    public Action<BuildingRate> onChangeBuilding { get; set; }
    public BuildService(PlayerDataService playerService, PropertyDataService propertyService)
    {
        this.playerService = playerService;
        this.propertyService = propertyService;
    }

    public void BuildNewHouse(int playerIndex, int spaceIndex)
    {
        playerService.SetCurrentCoin(playerIndex,
            -propertyService.GetCost(spaceIndex, PropertyDataService.BuildType.BuildNew));
        propertyService.AddBuilding(spaceIndex, PropertyDataService.BuildType.BuildNew);
        onChangeBuilding.Invoke(BuildingRate.D);
    }

    public void UpgradeHouses(int playerIndex, int spaceIndex, BuildingRate materialBuildingsRate, BuildingRate expectedBuildingRate)
    {
        playerService.SetCurrentCoin(playerIndex,
            -propertyService.GetCost(spaceIndex, PropertyDataService.BuildType.Upgrade));
        propertyService.AddBuilding(spaceIndex, PropertyDataService.BuildType.Upgrade, materialBuildingsRate, expectedBuildingRate);
        onChangeBuilding.Invoke(expectedBuildingRate);

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