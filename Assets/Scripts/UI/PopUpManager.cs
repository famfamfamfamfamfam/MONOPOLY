using UnityEngine;

public class PopupManager : MonoBehaviour, INeedDriver
{
    public Driver driver { get; set; }

    [SerializeField]
    NotifsPanel notifsPanel;
    [SerializeField]
    PurchaseOptionPanel purchaseOptionPanel;

    private void Start()
    {
        driver.AskToPurchaseSpace(() => Show(purchaseOptionPanel.gameObject));
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

public interface ITransientPopup
{
    void SetFadeDuration(float duration);
    void DisplayContent(string content);
}