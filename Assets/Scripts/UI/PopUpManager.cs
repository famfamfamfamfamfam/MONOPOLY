using UnityEngine;

public class PopupManager : MonoBehaviour, INeedDriver
{
    public Driver driver { get; set; }

    [SerializeField]
    NotifsPanel notifsPanel;
    [SerializeField]
    PurchaseOptionPanel purchaseOptionPanel;
    [SerializeField]
    InfoDisplayManager infoManager;
    [SerializeField]
    PropertyNameHandler propertyNameHandler;
    [SerializeField]
    ShowToggle toggle;

    private void Start()
    {
        driver.AskToPurchaseSpace(() => toggle.ShowHideToggle(purchaseOptionPanel.gameObject));
        driver.Notif(notifContent => Pop(notifsPanel.gameObject, notifContent, 2f));
        infoManager.PrepareToUnfold(toggle.ShowHideToggle);
        propertyNameHandler.OnFindNameSuccess(toggle.ShowHideToggle);
    }

    void Pop(GameObject panel, string content, float duration)
    {
        ITransientPopup popup = panel.GetComponent<ITransientPopup>();
        popup.SetFadeDuration(duration);
        popup.DisplayContent(content);
        panel.SetActive(true);
    }

}