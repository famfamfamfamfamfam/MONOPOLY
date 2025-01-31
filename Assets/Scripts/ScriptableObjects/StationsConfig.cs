using UnityEngine;

[CreateAssetMenu(fileName = "New Stations", menuName = "Scriptable Objects/Stations Config")]
public class StationsConfig : SpaceGroupConfig
{
    public int stationCount = 4;
    public int eachPurchaseCost = 200;
    public int eachInitialRentCost = 25;
    public int rentCostScaleFactor = 2;
}