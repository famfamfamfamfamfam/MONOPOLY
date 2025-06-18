using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionsPanel : MonoBehaviour, INeedDriver
{
    [SerializeField]
    Button upgradeDToC;
    [SerializeField]
    Button upgradeCToB;
    [SerializeField]
    Button upgradeBToA;

    public Driver driver { get; set; }

    void OnEnable()
    {
        SwitchBtn(upgradeBToA.gameObject, BuildingRate.B);
        SwitchBtn(upgradeCToB.gameObject, BuildingRate.C);
        SwitchBtn(upgradeDToC.gameObject, BuildingRate.D);
    }

    void Start()
    {
        upgradeBToA.onClick.AddListener(() => driver.UpgradeBuildings(BuildingRate.B, BuildingRate.A));
        upgradeBToA.onClick.AddListener(() => SwitchBtn(upgradeBToA.gameObject, BuildingRate.B));

        upgradeCToB.onClick.AddListener(() => driver.UpgradeBuildings(BuildingRate.C, BuildingRate.B));
        upgradeCToB.onClick.AddListener(() => SwitchBtn(upgradeBToA.gameObject, BuildingRate.B));
        upgradeCToB.onClick.AddListener(() => SwitchBtn(upgradeCToB.gameObject, BuildingRate.C));

        upgradeDToC.onClick.AddListener(() => driver.UpgradeBuildings(BuildingRate.D, BuildingRate.C));
        upgradeDToC.onClick.AddListener(() => SwitchBtn(upgradeCToB.gameObject, BuildingRate.C));
        upgradeDToC.onClick.AddListener(() => SwitchBtn(upgradeDToC.gameObject, BuildingRate.D));
    }
    
    void SwitchBtn(GameObject theRelatedDisplay, BuildingRate materialRate)
    {
        theRelatedDisplay.SetActive(driver.CanUpgrade(materialRate));
    }

    public void DisplayToUpgradeDBuildings()
    {
        SwitchBtn(upgradeDToC.gameObject, BuildingRate.D);
    }

    void OnDestroy()
    {
        upgradeBToA.onClick.RemoveAllListeners();
        upgradeCToB.onClick.RemoveAllListeners();
        upgradeDToC.onClick.RemoveAllListeners();
    }
}
