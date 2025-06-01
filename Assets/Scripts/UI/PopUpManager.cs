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
    ShowButton[] showButtons;

    private void Start()
    {
        driver.AskToPurchaseSpace(() => ShowHideToggle(purchaseOptionPanel.gameObject));
        driver.Notif(notifContent => Pop(notifsPanel.gameObject, notifContent, 2f));
        infoManager.PrepareToUnfold(ShowHideToggle);
        propertyNameHandler.OnFindNameSuccess(ShowHideToggle);
        for (int i = 0; i < showButtons.Length; i++)
        {
            showButtons[i].OnInteract(ShowHideToggle);
        }
    }

    void Pop(GameObject panel, string content, float duration)
    {
        ITransientPopup popup = panel.GetComponent<ITransientPopup>();
        popup.SetFadeDuration(duration);
        popup.DisplayContent(content);
        panel.SetActive(true);
    }

    void ShowHideToggle(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}