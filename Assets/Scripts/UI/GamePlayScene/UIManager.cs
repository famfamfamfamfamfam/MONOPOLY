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
        nameHandler.onFindAListOfPropertiesByKeyword = presenter.UpdatePricesToDisplay;    
    }
}
