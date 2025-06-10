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
    }
}

public enum BuildingRate : byte
{
    A = 0,
    B = 1,
    C = 2,
    D = 3
}