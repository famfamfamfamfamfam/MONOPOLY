using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    PropertyNameHandler nameHandler;
    [SerializeField]
    PropertyPricesPresenter presenter;

    public void Init(PropertyConfig[] propertyConfigs)
    {
        presenter.SetPropertyConfigs(propertyConfigs);
    }

    void Start()
    {
        nameHandler.onFoundAListOfPropertiesByKeyword = presenter.UpdatePricesToDisplay;
        nameHandler.onFoundAssetByFullName = presenter.UpdatePricesToDisplay;
    }
}
