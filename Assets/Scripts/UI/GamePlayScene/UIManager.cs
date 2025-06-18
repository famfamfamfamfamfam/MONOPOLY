using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    PropertyNameHandler nameHandler;
    [SerializeField]
    PropertyPricesPresenter presenter;

    [SerializeField]
    BuildOptionsPanel buildOptionsPanel;
    [SerializeField]
    UpgradeOptionsPanel upgradeOptionsPanel;

    public void Init(PropertyConfig[] propertyConfigs)
    {
        presenter.SetPropertyConfigs(propertyConfigs);
    }

    void Start()
    {
        nameHandler.onFoundAListOfPropertiesByKeyword = presenter.UpdatePricesToDisplay;
        nameHandler.onFoundAssetByFullName = presenter.UpdatePricesToDisplay;

        buildOptionsPanel.Init(upgradeOptionsPanel.DisplayToUpgradeDBuildings);
    }
}
