using TMPro;
using UnityEngine;

public class NotifsPanel : MonoBehaviour, ITransientPopup
{
    [SerializeField]
    CanvasGroup notifCanvasGroup;

    [SerializeField]
    TextMeshProUGUI notifContent;

    float fadeDuration = -1f;
    void Update()
    {
        if (fadeDuration < 0)
        {
            return;
        }

        if (notifCanvasGroup.alpha == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            notifCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
        }
    }

    public void DisplayContent(string content)
    {
        notifContent.text = content;
    }

    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }

    private void OnDisable()
    {
        notifCanvasGroup.alpha = 1;
        fadeDuration = -1f;
    }
}
