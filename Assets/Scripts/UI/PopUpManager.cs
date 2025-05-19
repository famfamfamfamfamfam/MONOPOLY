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

    private void Start()
    {
        driver.AskToPurchaseSpace(() => Show(purchaseOptionPanel.gameObject));
        driver.Notif(notifContent => Pop(notifsPanel.gameObject, notifContent, 2f));
        infoManager.PrepareToUnfold(Show);
    }

    void Pop(GameObject panel, string content, float duration)
    {
        ITransientPopup popup = panel.GetComponent<ITransientPopup>();
        popup.SetFadeDuration(duration);
        popup.DisplayContent(content);
        panel.SetActive(true);
    }

    void Show(GameObject panel)
    {
        panel.SetActive(true);
    }
}