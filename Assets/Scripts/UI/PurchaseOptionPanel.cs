using UnityEngine;
using UnityEngine.UI;

public class PurchaseOptionPanel : MonoBehaviour
{
    [SerializeField]
    DataManager dataManager;

    [SerializeField]
    Button noOption;
    [SerializeField]
    Button yesOption;

    void Start()
    {
        yesOption.onClick.AddListener(() =>
        {
            dataManager.PurchaseTheSpace();
            gameObject.SetActive(false);
        });
        noOption.onClick.AddListener(() =>
        {
            Debug.Log("No purchase");
            gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        yesOption.onClick.RemoveAllListeners();
        noOption.onClick.RemoveAllListeners();
    }

}
