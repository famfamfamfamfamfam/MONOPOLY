using UnityEngine;

[CreateAssetMenu(fileName = "New Stations", menuName = "Scriptable Objects/Stations Config")]
public class StationsConfig : SpaceGroupConfig
{
    public uint stationCount = 4;
    public uint eachPurchaseCost = 200;
    public uint eachInitialRentCost = 25;
    public uint rentCostScaleFactor = 2;
}