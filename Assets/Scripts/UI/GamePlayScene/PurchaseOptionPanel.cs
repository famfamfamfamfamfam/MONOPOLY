using UnityEngine;
using UnityEngine.UI;

public class PurchaseOptionPanel : MonoBehaviour, INeedDriver
{
    public Driver driver { get; set; }

    [SerializeField]
    Button noOption;
    [SerializeField]
    Button yesOption;

    void Start()
    {
        yesOption.onClick.AddListener(() =>
        {
            driver.PurchaseTheSpace();
            driver.AfterPurchaseDecision();
            gameObject.SetActive(false);
        });
        noOption.onClick.AddListener(() =>
        {
            Debug.Log("No purchase");
            driver.AfterPurchaseDecision();
            gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        yesOption.onClick.RemoveAllListeners();
        noOption.onClick.RemoveAllListeners();
    }

}
